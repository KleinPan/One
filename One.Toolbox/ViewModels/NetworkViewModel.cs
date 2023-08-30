using One.Toolbox.Enums;
using One.Toolbox.Helpers;

using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace One.Toolbox.ViewModels
{
    public partial class NetworkViewModel : BaseViewModel
    {
        [ObservableProperty]
        private CommunProtocalType selectCommunProtocalType;

        //收到消息的事件
        public event EventHandler<byte[]> DataRecived;

        public bool IsConnected { get; set; } = false;
        public bool HexMode { get; set; } = false;

        [ObservableProperty]
        private ObservableCollection<string> ipList = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedIP;

        [ObservableProperty]
        private string inputPort;

        //[ObservableProperty]
        //private string inputIP;

        public override void InitializeViewModel()
        {
            if (isInitialized)
                return;

            RefreshIp();
            //绑定

            InputPort = "8088";

            //收到消息，显示日志
            DataRecived += (name, data) =>
            {
                ShowData($" → receive ({(string)name})", data);
            };

            base.InitializeViewModel();
        }

        [ObservableProperty]
        private string dataToSend;

        #region Command

        [RelayCommand]
        private void SendData()
        {
            switch (SelectCommunProtocalType)
            {
                case CommunProtocalType.TCP客户端:

                    if (socketNow != null)
                    {
                        byte[] buff = HexMode ? ByteHelper.HexToByte(DataToSend) :
                            Tools.Global.GetEncoding().GetBytes(DataToSend);
                        Send(buff);
                    }

                    break;

                case CommunProtocalType.TCP服务端:

                    if (Server != null)
                    {
                        byte[] buff = HexMode ? ByteHelper.HexToByte(dataToSend) : Tools.Global.GetEncoding().GetBytes(dataToSend);
                        Broadcast(buff);
                    }

                    break;

                case CommunProtocalType.UDP客户端:
                    break;

                case CommunProtocalType.UDP服务端:
                    break;

                default:
                    break;
            }
        }

        [RelayCommand]
        private void StopListen()
        {
            try
            {
                StopServer();
                IsConnected = false;
                ShowData($"🚫 server closed");
            }
            catch { }
        }

        [RelayCommand]
        private void Listen()
        {
            int port;
            if (int.TryParse(InputPort, out port))
            {
                try
                {
                    IsConnected = StartServer(SelectedIP, port);
                }
                catch (Exception err)
                {
                    Tools.MessageBox.Show(err.Message);
                }
            }
        }

        /// <summary> 刷新本机ip列表 </summary>
        [RelayCommand]
        private void RefreshIp()
        {
            IpList.Clear();
            IpList.Add("0.0.0.0");
            IpList.Add("::");
            var temp = new List<string>();
            try
            {
                string name = Dns.GetHostName();
                IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
                foreach (IPAddress ipa in ipadrlist)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork ||
                        ipa.AddressFamily == AddressFamily.InterNetworkV6)
                        temp.Add(ipa.ToString());
                }
            }
            catch { }
            //去重
            temp.Distinct().ToList().ForEach(ip => IpList.Add(ip));
        }

        #endregion Command

        private bool Broadcast(byte[] buff)
        {
            try
            {
                lock (Clients)
                {
                    foreach (var c in Clients)
                        try
                        {
                            c.Send(buff);
                        }
                        catch { }
                }
                ShowData($"💥 broadcast", buff, true);
                return true;
            }
            catch (Exception ex)
            {
                ShowData($"❗ broadcast error {ex.Message}");
                return false;
            }
        }

        private void ShowData(string title, byte[] data = null, bool send = false)
        {
            /*
            Tools.Logger.ShowDataRaw(new Tools.DataShowRaw
            {
                title = $"🛰 local tcp server: {title}",
                data = data ?? new byte[0],
                color = send ? Brushes.DarkRed : Brushes.DarkGreen,
            });
            */
        }

        /// <summary> 获取客户端的名字 </summary>
        /// <param name="s"> </param>
        /// <returns> </returns>
        private string GetClientName(Socket s)
        {
            var remote = (IPEndPoint)s.RemoteEndPoint;
            var remoteIsV6 = remote.Address.ToString().Contains(":");
            var local = (IPEndPoint)s.LocalEndPoint;
            var localIsV6 = local.Address.ToString().Contains(":");
            return $"{(remoteIsV6 ? "[" : "")}{remote.Address}{(remoteIsV6 ? "]" : "")}:{remote.Port} → " +
                $"{(localIsV6 ? "[" : "")}{local.Address}{(localIsV6 ? "]" : "")}:{local.Port}";
        }

        private TcpListener Server = null;
        private List<Socket> Clients = new List<Socket>();

        public void Read_Callback(IAsyncResult ar)
        {
            StateObject so = (StateObject)ar.AsyncState;
            Socket s = so.workSocket;
            try
            {
                var name = GetClientName(s);
                int read = s.EndReceive(ar);

                if (read > 0)
                {
                    var buff = new byte[read];
                    for (int i = 0; i < buff.Length; i++)
                        buff[i] = so.buffer[i];
                    DataRecived?.Invoke(name, buff);
                    s.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, 0,
                                             new AsyncCallback(Read_Callback), so);
                }
            }
            catch//断了？
            {
                try
                {
                    var name = GetClientName(s);
                    lock (Clients)
                        Clients.Remove(s);
                    try
                    {
                        s.Close();
                        s.Dispose();
                    }
                    catch { }
                    ShowData($"☠ {name}");
                }
                catch { }
            }
        }

        /// <summary> 开始监听服务器 </summary>
        /// <param name="ip">   </param>
        /// <param name="port"> </param>
        /// <returns> </returns>
        private bool StartServer(string ip, int port)
        {
            if (Server != null)
                return false;
            IPAddress localAddr = IPAddress.Parse(ip);
            Server = new TcpListener(localAddr, port);
            Server.Start();
            var isV6 = ip.Contains(":");
            ShowData($"🛰 {(isV6 ? "[" : "")}{ip}{(isV6 ? "]" : "")}:{port}");
            AsyncCallback newConnectionCb = null;
            newConnectionCb = new AsyncCallback((ar) =>
            {
                TcpListener listener = (TcpListener)ar.AsyncState;
                try
                {
                    Socket client = listener.EndAcceptSocket(ar);//必须有这一句，不然新的请求没反应
                    ShowData($"😀 {GetClientName(client)}");
                    lock (Clients)
                        Clients.Add(client);//加到列表里

                    //客户端数据接收回调
                    StateObject so = new StateObject();
                    so.workSocket = client;
                    client.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(Read_Callback), so);

                    //恢复服务端的回调函数，方便下次接收
                    Server.BeginAcceptSocket(newConnectionCb, Server);
                }
                catch { }
            });
            try
            {
                Server.BeginAcceptSocket(newConnectionCb, Server);
            }
            catch (Exception ex)
            {
                ShowData($"❗ Server create error {ex.Message}");
                return false;
            }

            return true;
        }

        /// <summary> 关闭服务器，断开所有连接 </summary>
        private void StopServer()
        {
            lock (Clients)
            {
                foreach (var c in Clients)
                    try
                    {
                        var name = GetClientName(c);
                        c.Close();
                        c.Dispose();
                        ShowData($"☠ {name}");
                    }
                    catch { }
                Clients.Clear();
            }
            Server?.Stop();
            Server = null;
        }

        #region Soket

        //是否可更改服务器信息
        public bool Changeable { get; set; } = true;

        //暂存一个对象
        private Socket socketNow = null;

        private void SocketLoaded()
        {
            //收到消息显示
            DataRecived += (_, buff) =>
            {
                ShowData($" → receive", buff);
            };

            //适配一下通用通道
            /*
            LuaApis.SendChannelsRegister("socket-client", (data, _) =>
            {
                if (socketNow != null && data != null)
                {
                    return Send(data);
                }
                else
                    return false;
            });
            //通用通道收到消息
            DataRecived += (_, data) =>
            {
                LuaApis.SendChannelsReceived("socket-client", data);
            };
            */
        }

        public void SocketRead_Callback(IAsyncResult ar)
        {
            StateObject so = (StateObject)ar.AsyncState;
            Socket s = so.workSocket;
            try
            {
                int read = s.EndReceive(ar);

                if (read > 0)
                {
                    var buff = new byte[read];
                    for (int i = 0; i < buff.Length; i++)
                        buff[i] = so.buffer[i];
                    DataRecived?.Invoke(null, buff);
                    s.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, 0,
                                             new AsyncCallback(SocketRead_Callback), so);
                }
                else//断了？
                {
                    try
                    {
                        s.Close();
                        s.Dispose();
                    }
                    catch { }
                    socketNow = null;
                    IsConnected = false;
                    Changeable = true;
                    ShowData("❌ Server disconnected");
                }
            }
            catch { }
        }

        [RelayCommand]
        private void SocketConnect()
        {
            if (!Changeable)
                return;
            IPEndPoint ipe = null;
            Socket s = null;
            try
            {
                Changeable = false;
                IPAddress ip = null;
                try
                {
                    ip = IPAddress.Parse(SelectedIP);
                }
                catch
                {
                    var hostEntry = Dns.GetHostEntry(SelectedIP);
                    ip = hostEntry.AddressList[0];
                }
                ipe = new IPEndPoint(ip, int.Parse(InputPort));

                switch (SelectCommunProtocalType)
                {
                    case CommunProtocalType.TCP客户端:

                        s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        break;

                    case CommunProtocalType.TCP服务端:
                        break;

                    case CommunProtocalType.UDP客户端:

                        s = new Socket(ipe.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                        break;

                    case CommunProtocalType.UDP服务端:
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowData($"❗ Server information error {ex.Message}");
                Changeable = true;
                return;
            }
            ShowData("📢 Connecting......");
            try
            {
                s.BeginConnect(ipe, new AsyncCallback((r) =>
                {
                    var s = (Socket)r.AsyncState;
                    if (s.Connected)
                    {
                        socketNow = s;
                        IsConnected = true;
                        ShowData("✔ Server connected");
                    }
                    else
                    {
                        Changeable = true;
                        ShowData("❗ Server connect failed");
                        return;
                    }

                    StateObject so = new StateObject();
                    so.workSocket = s;
                    s.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(Read_Callback), so);
                }), s);
            }
            catch (Exception ex)
            {
                ShowData($"❗ Server connect error {ex.Message}");
                Changeable = true;
                return;
            }
        }

        [RelayCommand]
        private void SocketDisconnect()
        {
            if (socketNow != null)
            {
                try
                {
                    socketNow.Close();
                    socketNow.Dispose();
                }
                catch { }
                socketNow = null;
                IsConnected = false;
                Changeable = true;
                ShowData("❌ Server disconnected");
            }
        }

        private bool Send(byte[] buff)
        {
            try
            {
                socketNow.Send(buff);
                ShowData($" ← send", buff, true);
                return true;
            }
            catch (Exception ex)
            {
                ShowData($"❗ Send data error {ex.Message}");
                return false;
            }
        }

        public class StateObject
        {
            public Socket workSocket = null;
            public const int BUFFER_SIZE = 2048;
            public byte[] buffer = new byte[BUFFER_SIZE];
        }

        #endregion Soket
    }
}