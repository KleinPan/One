using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace One.Core.Helpers.NetHelpers
{
    /// <summary>
    /// 使用新版本微软推荐的异步套接字开发
    /// <para> https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services#create-a-socket-client </para>
    /// <para> https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=net-7.0 </para>
    /// </summary>
    public class AsyncTCPClient : BaseHelper
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
        public Action<byte[]> OnConnected;

        public CancellationToken cancellationToken = default;

        /// <summary> 暂时不起作用 </summary>
        public int WaitTime = 100;

        public AsyncTCPClient(Action<string> logAction) : base(logAction)
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

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.RemoteEndPoint = ipEndPoint;
                e.UserToken = socket;
                e.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectComplete);
                e.SetBuffer(System.Text.Encoding.UTF8.GetBytes("Client connect succeed!"));

                var willRaiseEvent = socket.ConnectAsync(e);

                if (!willRaiseEvent)//暂时没发现有什么用
                {
                    ConnectComplete(this, e);
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
                //客户端自己的Socket
                var localClientSocket = e.ConnectSocket;//连接的 Socket 对象。
                var addressFamily = localClientSocket.AddressFamily.ToString();
                // var a = localClientSocket.LocalEndPoint.ToString();
                var a = $"{localClientSocket.LocalEndPoint.ToString()} connected!";

                var info = System.Text.Encoding.UTF8.GetBytes(a);
                OnConnected?.Invoke(info);

                WriteLog(a);

                //连接成功再启动接受函数
                Receive();
            }
            catch (Exception ex)
            {
                WriteLog($"连接失败 => {ex}");

                // throw;
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

                SendAction?.Invoke(data);
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
                    if (!socket.Connected)
                    {
                        return;
                    }
                    byte[] responseBytes = new byte[1024];
                    int bytesReceived = await socket.ReceiveAsync(responseBytes, SocketFlags.None, cancellationToken);

                    if (bytesReceived > 0)
                    {
                        ReceiveAction?.Invoke(responseBytes.Take(bytesReceived).ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }
    }
}