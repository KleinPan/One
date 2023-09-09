using Org.BouncyCastle.Asn1.Ocsp;

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace One.Core.Helpers.NetHelpers
{
    /// <summary>
    /// 使用新版本微软推荐的异步套接字开发
    /// <para> https://learn.microsoft.com/zh-cn/dotnet/api/system.net.sockets.udpclient?view=net-7.0 </para>
    /// </summary>
    public class AsyncUDPClient : BaseHelper
    {
        #region 变量

        private UdpClient udpClient = null;

        #endregion 变量

        public Action<string, byte[]> ReceiveAction;

        public Action<byte[]> SendAction;
        public Action<byte[]> OnConnected;
        public Action<byte[]> OnDisConnected;

        public Action<IPListOperationEnum, string> OnIpEndPointChanged;

        public CancellationToken cancellationToken = default;

        /// <summary> 暂时不起作用 </summary>
        public int WaitTime = 100;

        public AsyncUDPClient(Action<string> logAction) : base(logAction)
        {
        }

        /// <summary> 初始化作为客户端并连接 </summary>
        /// <param name="ip">   </param>
        /// <param name="port"> </param>
        public bool InitClient(IPAddress ip, int port)
        {
            try
            {
                IPEndPoint ipEndPoint = new(ip, port);

                udpClient = new UdpClient(ipEndPoint);

                //udpClient.Connect(ipEndPoint);

                var local = udpClient.Client.LocalEndPoint;
                var info = System.Text.Encoding.UTF8.GetBytes($"Local {local} is listening!");
                OnConnected?.Invoke(info);
                Receive();
                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
        }

        /// <summary> 释放当前客户端连接 </summary>
        /// <returns> </returns>
        public void ReleaseClient()
        {
            try
            {
                //注意对于udp协议来说，仍然接受并排列传入的数据，因此udp套接字而言shutdown毫无意义
                udpClient.Close();

                var info = System.Text.Encoding.UTF8.GetBytes("DD");
                OnDisConnected?.Invoke(info);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        public async void SendData(IPEndPoint iPEndPoint, byte[] data)
        {
            try
            {
                // int count = sckClient.Send(data);
                //Console.WriteLine("发送数据长度为：" + count);

                int bytesSent = 0;
                while (bytesSent < data.Length)
                {
                    bytesSent += await udpClient.SendAsync(data.AsMemory(bytesSent), iPEndPoint);
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
                    var receive = await udpClient.ReceiveAsync(cancellationToken);

                    if (receive.Buffer.Length > 0)
                    {
                        var remoteEndpoint = receive.RemoteEndPoint.ToString();
                        OnIpEndPointChanged?.Invoke(IPListOperationEnum.Add, remoteEndpoint);

                        var msg = receive.Buffer.ToArray();
                        ReceiveAction?.Invoke(remoteEndpoint, msg);
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