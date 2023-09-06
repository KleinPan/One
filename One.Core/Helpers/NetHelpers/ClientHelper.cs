using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg;

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace One.Core.Helpers.NetHelpers
{
    /// <summary>
    /// 使用新版本微软推荐的异步套接字开发
    /// <para> https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services#create-a-socket-client </para>
    /// <para> https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=net-7.0 </para>
    /// </summary>
    public class ClientHelper : BaseHelper
    {
        #region 变量

        private Socket socket = null;

        /// <summary> 原始字节数据 </summary>
        private byte[] receiveBuffer = new byte[1024];

        /// <summary> 发送数据存储区 </summary>
        public string DataSend = "";

        #endregion 变量

        //private Socket sendSocket = null;
        public Action<byte[]> ReceiveAction;

        public Action<byte[]> SendAction;
        public Action OnConnected;

        public CancellationToken cancellationToken = default;

        /// <summary> 暂时不起作用 </summary>
        public int WaitTime = 100;

        public ClientHelper(Action<string> logAction) : base(logAction)
        {
        }

        /// <summary> 初始化作为客户端并连接 </summary>
        /// <param name="ip">   </param>
        /// <param name="port"> </param>
        public bool InitAsClient(IPAddress ip, int port)
        {
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(ip, port);

                socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.RemoteEndPoint = ipEndPoint;
                e.UserToken = socket;
                e.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectComplete);
                e.SetBuffer(System.Text.Encoding.UTF8.GetBytes("Client connect succeed!"));

                var willRaiseEvent = socket.ConnectAsync(e);

                if (!willRaiseEvent)//暂时没发现有什么用
                {
                    ConnectEvent(e);
                }

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
        }

        /// <summary> 连接成功事件 </summary>
        /// <param name="sender"> </param>
        /// <param name="e">      </param>
        private void ConnectComplete(object sender, SocketAsyncEventArgs e)
        {
            //这里设置buffer已经晚了，所以提前设置进去

            try
            {
                var addressFamily = e.ConnectSocket.AddressFamily.ToString();
                var a = e.ConnectSocket.LocalEndPoint.ToString();
                WriteLog(addressFamily + " " + a);

                OnConnected?.Invoke();

                //连接成功再启动接受函数
                Receive();
            }
            catch (Exception ex)
            {
                WriteLog($"连接失败 => {ex.ToString()}");

                throw;
            }
        }

        private void ConnectEvent(SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError == SocketError.Success)
                {
                }
                else
                {
                }
            }
            finally
            {
                args.Dispose();
            }
        }

        /// <summary> 断开当前客户端连接 </summary>
        /// <returns> </returns>
        public void UnInitAsClient()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        private void EndConnectCallback(IAsyncResult ar)
        {
            try
            {
                socket.EndDisconnect(ar);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        public async void SendData(byte[] data)
        {
            try
            {
                // int count = sckClient.Send(data);
                //Console.WriteLine("发送数据长度为：" + count);

                int bytesSent = 0;
                while (bytesSent < data.Length)
                {
                    bytesSent += await socket.SendAsync(data.AsMemory(bytesSent), SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        /// <summary> 循环接受缓冲区数据 </summary>
        private async void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] responseBytes = new byte[1024];
                    int bytesReceived = await socket.ReceiveAsync(responseBytes, SocketFlags.None, cancellationToken);

                    // Receiving 0 bytes means EOF has been reached
                    //if (bytesReceived == 0) break;

                    if (bytesReceived > 0)
                    {
                        ReceiveAction?.Invoke(responseBytes.Take(bytesReceived).ToArray());
                    }
                    //Thread.Sleep(WaitTime);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }
    }
}