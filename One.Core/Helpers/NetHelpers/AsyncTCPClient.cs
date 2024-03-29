using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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

        #endregion 变量

        public Action<byte[]> ReceiveAction;
        public Action<byte[]> SendAction;
        public Action<byte[]> OnConnected;
        public Action<byte[]> OnDisConnected;
        public Action<bool> OnConnectedBool;
        public CancellationToken cancellationToken = default;

        /// <summary> 暂时不起作用 </summary>
        public int WaitTime = 100;

        public AsyncTCPClient(Action<string> logAction) : base(logAction)
        {
        }

        /// <summary> 初始化作为客户端并连接 </summary>
        /// <param name="ip">   </param>
        /// <param name="port"> </param>
        public void Connect(IPAddress ip, int port)
        {
            try
            {
                IPEndPoint ipEndPoint = new(ip, port);

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.RemoteEndPoint = ipEndPoint;
                e.UserToken = socket;

                e.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectComplete);
                //e.SetBuffer(System.Text.Encoding.UTF8.GetBytes("Hello!"));//发这个会出意外，本地打印log就行

                var pending = socket.ConnectAsync(e);

                var a = $"{socket.LocalEndPoint.ToString()} connecting......";
                var info = System.Text.Encoding.UTF8.GetBytes(a);
                OnConnected?.Invoke(info);
                if (!pending)//同步的走这里
                {
                    ConnectComplete(this, e);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
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
                //客户端自己的Socket,也可以直接用最开始的socket
                var localClientSocket = e.ConnectSocket;//连接的 Socket 对象。

                var addressFamily = localClientSocket.AddressFamily.ToString();

                OnConnectedBool?.Invoke(localClientSocket.Connected);
                Debug.WriteLine("Status:" + localClientSocket.Connected);

                var a = $"{localClientSocket.LocalEndPoint.ToString()} connected!";

                Debug.WriteLine(localClientSocket.RemoteEndPoint.ToString());

                var info = System.Text.Encoding.UTF8.GetBytes(a);
                OnConnected?.Invoke(info);

                WriteLog(a);

                //连接成功再启动接受函数
                Receive();
            }
            catch (Exception ex)
            {
                OnConnectedBool?.Invoke(false);

                var a = $"{e.RemoteEndPoint.ToString()} connect failed!";

                var info = System.Text.Encoding.UTF8.GetBytes(a);
                OnConnected?.Invoke(info);
                WriteLog($"连接失败 => {ex}");
            }
        }

        /// <summary> 释放当前客户端连接 </summary>
        /// <returns> </returns>
        public void ReleaseClient()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                OnConnectedBool?.Invoke(false);

                var a = $"{socket.LocalEndPoint} disconnected!";
                var info = System.Text.Encoding.UTF8.GetBytes(a);
                OnDisConnected?.Invoke(info);

                socket.Close();
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                throw;
            }
        }

        public async void SendData(byte[] data)
        {
            try
            {
                if (!socket.Connected)
                {
                    OnConnectedBool?.Invoke(false);

                    return;
                }
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
                //不能上抛
                WriteLog(ex.ToString());
                //throw ex;
            }
        }
    }
}