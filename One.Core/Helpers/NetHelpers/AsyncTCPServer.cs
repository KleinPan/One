using One.Core.ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace One.Core.Helpers.NetHelpers
{
    public class AsyncTCPServer : BaseHelper
    {
        #region 变量

        /// <summary> 服务器套接字 </summary>
        private Socket sckServer = null;

        public Socket sckSend = null;

        public List<Socket> listConnection = new List<Socket>();

        #endregion 变量

        #region Action

        public Action<string, byte[]> ReceiveAction;

        public Action<byte[]> SendAction;
        public Action<byte[]> OnConnected;
        public Action<byte[]> OnDisConnected;

        public bool IsStart = false;

        #endregion Action

        public AsyncTCPServer(Action<string> logAction) : base(logAction)
        {
        }

        /// <summary> 初始化作为服务器并启动 </summary>
        /// <param name="ip">   </param>
        /// <param name="port"> </param>
        /// <returns> </returns>
        public bool InitAsServer(IPAddress ip, int port)
        {
            try
            {
                IPEndPoint iep = new IPEndPoint(ip, port);
                Start(iep);

                RefreshConnections();
                IsStart = true;

                return true;
            }
            catch (Exception ex)
            {
                WriteLog($"{ex}");
                return false;
            }
        }

        void Start(IPEndPoint iPEndPoint)
        {
            //创建服务器套接字
            sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //绑定
            sckServer.Bind(iPEndPoint);
            Console.WriteLine("绑定成功！");

            //监听
            sckServer.Listen(5);
            Console.WriteLine("监听成功！");

            Console.WriteLine("等待接受连接请求！");

            StartAccept();
        }

        void StartAccept()
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();

            e.UserToken = sckServer;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(OnAccept);

            //如果 I/O 操作挂起，则为 true。 操作完成时，将引发 e 参数的 Completed 事件。
            //如果 I/O 操作同步完成，则为 false。 将不会引发 e 参数的 Completed 事件，并且可能在方法调用返回后立即检查作为参数传递的 e 对象以检索操作的结果。
            var willRaiseEvent = sckServer.AcceptAsync(e);

            if (!willRaiseEvent)
            {
                OnAccept(this, e);
            }
        }

        private EventWaitHandle disconnect = new AutoResetEvent(false);

        private void OnAccept(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError != SocketError.Success)
                {
                    return;
                }
                else
                {
                }
                disconnect.Reset();

                //服务端分配的Socket
                var localServerSocket = args.AcceptSocket;
                listConnection.Add(localServerSocket);

                disconnect.Set();//其他地方可以处理了

                var addressFamily = localServerSocket.RemoteEndPoint.AddressFamily.ToString();
                var a = $"{localServerSocket.RemoteEndPoint} connected!";

                var info = System.Text.Encoding.UTF8.GetBytes(a);
                OnConnected?.Invoke(info);
                WriteLog(a);
                //连接成功再启动接受函数

                StartAccept();
                Receive(localServerSocket);
            }
            catch (Exception ex)
            {
                WriteLog($"连接失败 => {ex}");
            }
        }

        /// <summary> 检测链接是否断开 </summary>
        private void RefreshConnections()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    disconnect.WaitOne();

                    for (int i = listConnection.Count - 1; i >= 0; i--)
                    {
                        var res = NetHelper.IsSocketConnect(listConnection[i]);
                        if (!res)
                        {
                            var addressFamily = listConnection[i].RemoteEndPoint.AddressFamily.ToString();
                            var a = $"{listConnection[i].RemoteEndPoint} disconnected!";

                            listConnection.RemoveAt(i);

                            var info = System.Text.Encoding.UTF8.GetBytes(a);
                            OnDisConnected?.Invoke(info);
                        }
                    }
                    Thread.Sleep(100 * 1);
                    disconnect.Set();
                }
            });
        }

        /// <summary> 循环接受缓冲区数据 </summary>
        private async void Receive(Socket thisSocket)
        {
            try
            {
                while (true)
                {
                    if (!thisSocket.Connected)
                    {
                        return;
                    }

                    byte[] responseBytes = new byte[1024];
                    int bytesReceived = await thisSocket.ReceiveAsync(responseBytes, SocketFlags.None);

                    if (bytesReceived > 0)
                    {
                        var remoteEndpoint = thisSocket.RemoteEndPoint.ToString();

                        var msg = responseBytes.Take(bytesReceived).ToArray();
                        ReceiveAction?.Invoke(remoteEndpoint, msg);
                    }

                    //Thread.Sleep(WaitTime);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        public async void SendDataToAll(byte[] data)
        {
            try
            {
                foreach (var item in listConnection)
                {
                    int bytesSent = 0;
                    while (bytesSent < data.Length)
                    {
                        bytesSent += await item.SendAsync(data.AsMemory(bytesSent), SocketFlags.None);
                    }
                }
                SendAction?.Invoke(data);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        /// <summary> 关闭服务器，断开所有连接 </summary>
        public void StopServer()
        {
            lock (listConnection)
            {
                foreach (var c in listConnection)
                    try
                    {
                        //c.Close();
                        //c.Dispose();
                        c.Shutdown(SocketShutdown.Both);
                    }
                    catch { }
                listConnection.Clear();
            }
            sckServer.Shutdown(SocketShutdown.Both);
            sckServer = null;
        }

        #region 传统写法

        /// <summary> 初始化作为服务器并启动 </summary>
        /// <param name="ip">   </param>
        /// <param name="port"> </param>
        /// <returns> </returns>
        public void InitAsServerOld(IPAddress ip, int port)
        {
            try
            {
                IPEndPoint iep = new IPEndPoint(ip, port);

                //创建服务器套接字
                sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //绑定
                sckServer.Bind(iep);
                Console.WriteLine("绑定成功！");

                //监听
                sckServer.Listen(5);
                Console.WriteLine("监听成功！");

                //接受连接请求
                sckServer.BeginAccept(new AsyncCallback(ConnectedCallback), sckServer);
            }
            catch (Exception ex)
            {
                WriteLog($"{ex}");
            }
        }

        //注意
        //需要重新写服务器,多个客户端连接要记录服务器的socket

        /// <summary> 接受连接请求回调函数—— 连接完成后执行 </summary>
        /// <param name="ar"> </param>
        private void ConnectedCallback(IAsyncResult ar)
        {
            try
            {
                //获取用户定义的对象，该对象限定或包含有关异步操作的信息。
                Socket sock = (Socket)ar.AsyncState;
                // 异步接受传入连接尝试并创建新的System.Net.Sockets.Socket以处理远程主机通信。
                Socket sckAccept = sock.EndAccept(ar);

                Console.WriteLine(sckAccept.RemoteEndPoint);
                //收集连接对
                listConnection.Add(sckAccept);

                sckSend = sckAccept;//接收和发送用的是一个套接字（连接）
                SendDataOld("Connected!");
                //Console.WriteLine("连接成功！");
                //清空接收数据缓冲区
                //Array.Clear(ReceiveBuf, 0, 255);

                var ReceiveBuf = new byte[1024];
                //接收数据
                sckSend.BeginReceive(ReceiveBuf, 0, 1024, SocketFlags.None, new AsyncCallback(ServeReceivedCallback), sckSend);

                string strReceive = Encoding.UTF8.GetString(ReceiveBuf, 0, 1024);
                ReceiveAndSend(strReceive);
                //接受连接请求
                sckServer.BeginAccept(new AsyncCallback(ConnectedCallback), sckServer);
            }
            catch (Exception e)
            {
                WriteLog(e.ToString());
            }
        }

        /// <summary> 接收数据回调函数—— 接收完显示用 </summary>
        /// <param name="ar"> </param>
        private void ServeReceivedCallback(IAsyncResult ar)
        {
            try
            {
                Socket sckReceive = (Socket)ar.AsyncState;
                int revLength = sckReceive.EndReceive(ar);

                //把接收到的数据转成字符串显示到界面
                //string strReceive = Encoding.UTF8.GetString(ReceiveBuf, 0, revLength);
                // var DataReceived = strReceive;
                var ReceiveBuf = new byte[1024];

                //再次接收数据
                sckReceive.BeginReceive(ReceiveBuf, 0, 1024, SocketFlags.None, new AsyncCallback(ServeReceivedCallback), sckReceive);

                string strReceive = Encoding.UTF8.GetString(ReceiveBuf, 0, 1024);
                ReceiveAndSend(strReceive);
            }
            catch (Exception e)
            {
                WriteLog(e.ToString());
            }
        }

        private void SeverEndConnectCallback(IAsyncResult ar)
        {
            try
            {
                sckServer.EndDisconnect(ar);
                listConnection.Clear();
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        /// <summary> 发送数据 </summary>
        /// <param name="data"> </param>
        public void SendDataOld(string data)
        {
            var SendBuf = Encoding.UTF8.GetBytes(data);

            sckSend.BeginSend(SendBuf, 0, SendBuf.Length, SocketFlags.None, new AsyncCallback(SendCallback), sckSend);
        }

        /// <summary> 发送数据回调函数 </summary>
        /// <param name="ar"> </param>
        private void SendCallback(IAsyncResult ar)
        {
            Socket sckSend = (Socket)ar.AsyncState;
            int sendLen = sckSend.EndSend(ar);
        }

        /// <summary> 需要在外部重写对消息格式 最后需要加上SendData(string data) </summary>
        /// <param name="mes"> 收到的消息 </param>
        public virtual void ReceiveAndSend(string mes)
        {
        }

        #region 前台

        //<ComboBox Name = "cmbSever" SelectionChanged="cmbSever_SelectionChanged">
        //        <ComboBox.ItemTemplate>
        //            <DataTemplate>
        //                <StackPanel Orientation = "Horizontal" >
        //                    < TextBlock Text="{Binding RemoteEndPoint}" />
        //                </StackPanel>
        //            </DataTemplate>
        //        </ComboBox.ItemTemplate>
        //    </ComboBox>

        #endregion 前台

        #region 后台

        //cmbSever.ItemsSource = server.listConnection;
        //server.sckSend = server.listConnection[cmbSever.SelectedIndex];

        #endregion 后台

        #endregion 传统写法
    }
}