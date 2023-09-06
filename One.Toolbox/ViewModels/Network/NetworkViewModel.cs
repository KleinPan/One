﻿using One.Core.Helpers.DataProcessHelpers;
using One.Core.Helpers.NetHelpers;
using One.Toolbox.Component;
using One.Toolbox.Enums;
using One.Toolbox.Helpers;

using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;

namespace One.Toolbox.ViewModels.Network
{
    public partial class NetworkViewModel : BaseViewModel
    {
        [ObservableProperty]
        private CommunProtocalType selectCommunProtocalType;

        //收到消息的事件
        public event EventHandler<byte[]> DataRecived;

        [ObservableProperty]
        private bool isConnected = false;

        [ObservableProperty]
        private bool hexMode = false;

        [ObservableProperty]
        private ObservableCollection<string> ipList = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedIP;

        [ObservableProperty]
        private string inputPort;

        private ClientHelper ClientHelper;

        public override void InitializeViewModel()
        {
            if (isInitialized)
                return;

            RefreshIp();
            //绑定

            InputPort = "2333";

            //收到消息，显示日志
            DataRecived += (name, data) =>
            {
                ShowData($" → receive ({(string)name})", data);
            };

            ClientHelper = new ClientHelper(WriteDebugLog);
            ClientHelper.ReceiveAction += ShowClientReceiveAction;
            base.InitializeViewModel();
        }

        private void ShowClientReceiveAction(byte[] data)
        {
            ShowData("", data, false);
        }

        [ObservableProperty]
        private string dataToSend;

        #region InitUI

        private FlowDocumentComponent flowDocumentHelper { get; set; }

        [RelayCommand]
        private void InitFlowDocumentControl(object obj)
        {
            var args = obj as System.Windows.RoutedEventArgs;
            var control = args.OriginalSource as FlowDocumentScrollViewer;
            flowDocumentHelper = new FlowDocumentComponent(control);
        }

        #endregion InitUI

        #region Command

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

        #endregion Command

        #region 通用功能

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
            temp.Distinct().Reverse().ToList().ForEach(ip => IpList.Add(ip));
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

                        //s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        ClientHelper.InitAsClient(ip, int.Parse(InputPort));
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

                IsConnected = true;
            }
            catch (Exception ex)
            {
                MessageShowHelper.ShowErrorMessage($"Server information error {ex.Message}");
                Changeable = true;
                return;
            }
            ShowData("Connecting......");
        }

        [RelayCommand]
        private void SocketDisconnect()
        {
            switch (SelectCommunProtocalType)
            {
                case CommunProtocalType.TCP客户端:
                    {
                        ClientHelper.UnInitAsClient();
                    }

                    break;

                case CommunProtocalType.TCP服务端:
                    break;

                case CommunProtocalType.UDP客户端:
                    break;

                case CommunProtocalType.UDP服务端:
                    break;

                default:
                    break;
            }

            IsConnected = false;
            Changeable = true;

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

        [RelayCommand]
        private void SendData()
        {
            switch (SelectCommunProtocalType)
            {
                case CommunProtocalType.TCP客户端:

                    {
                        byte[] buff = HexMode ? StringHelper.HexStringToBytes(DataToSend) ://ByteHelper.HexToByte(DataToSend)
                          Tools.Global.GetEncoding().GetBytes(DataToSend);

                        ShowData("", buff, true);
                        ClientHelper.SendData(buff);
                    }

                    break;

                case CommunProtocalType.TCP服务端:

                    if (Server != null)
                    {
                        byte[] buff = HexMode ? StringHelper.HexStringToBytes(DataToSend) : Tools.Global.GetEncoding().GetBytes(DataToSend);
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

        #endregion 通用功能

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
            if (data == null)
            {
                return;
            }
            string realData = "";
            if (HexMode)
            {
                realData = StringHelper.BytesToHexString(data);
            }
            else
            {
                realData = System.Text.Encoding.UTF8.GetString(data);
            }

            flowDocumentHelper.DataShowAdd(new Models.DataShowPara()
            {
                data = realData,
                send = send,
            });

            WriteInfoLog(realData);
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

        //是否可以进行操作，例如连接或者断开
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