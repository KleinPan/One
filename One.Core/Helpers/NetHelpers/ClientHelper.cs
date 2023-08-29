using One.Core.Helpers;

using System;
using System.Net;
using System.Net.Sockets;

namespace One.Core.Helpers.NetHelpers
{
    public class ClientHelper : BaseHelper
    {
        #region 变量

        public Socket sckClient = null;

        /// <summary> 原始字节数据 </summary>
        public byte[] ReceiveBuf = new byte[1024];

        private byte[] SendBuf = new byte[1024];

        /// <summary> 发送数据存储区 </summary>
        public string DataSend = "";

        public Socket sendSocket = null;

        #endregion 变量

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
                IPEndPoint iep = new IPEndPoint(ip, port);

                //创建客户端套接字
                sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //连接服务器
                sckClient.BeginConnect(iep, new AsyncCallback(ConnectCallback), sckClient);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary> 断开当前客户端连接 </summary>
        /// <returns> </returns>
        public bool UnInitAsClient()
        {
            try
            {
                //连接服务器
                sckClient.BeginDisconnect(true, new AsyncCallback(EndConnectCallback), sckClient);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary> 连接服务器回调函数 </summary>
        /// <param name="ar"> </param>
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket sckConnect = (Socket)ar.AsyncState;
                sckConnect.EndConnect(ar);
                //连接成功提示
                sendSocket = sckConnect;
            }
            catch (Exception e)
            {
                WriteLog(e.ToString());
            }
            //清空接收数据缓冲区
            //Array.Clear(ReceiveBuf, 0, 256);
            Array.Clear(ReceiveBuf, 0, ReceiveBuf.Length);

            try
            {
                //接收数据
                sckClient.BeginReceive(ReceiveBuf, 0, ReceiveBuf.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), sckClient);
                //int a = sckClient.Receive(ReceiveBuf);
            }
            catch (Exception e)
            {
                WriteLog(e.ToString());
            }
        }

        private void EndConnectCallback(IAsyncResult ar)
        {
            try
            {
                sckClient.EndDisconnect(ar);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        public byte[] Receive()
        {
            try
            {
                Array.Clear(ReceiveBuf, 0, ReceiveBuf.Length);

                int count = sckClient.Receive(ReceiveBuf);
                Console.WriteLine("接收数据长度为：" + count);
                return ReceiveBuf;
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return null;
            }
        }

        /// <summary> 接收数据回调函数 </summary>
        /// <param name="ar"> </param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket sckReceive = (Socket)ar.AsyncState;
            int revLength = sckReceive.EndReceive(ar);

            Console.WriteLine("接收数据长度为：" + revLength);
            //sendSocket = sckReceive.EndAccept(ar);

            //把接收到的数据转成字符串显示到界面
            //string strReceive = Encoding.UTF8.GetString(ReceiveBuf, 0, revLength);

            ReceiveAndSend(ReceiveBuf);

            //再次接收数据
            sckClient.BeginReceive(ReceiveBuf, 0, ReceiveBuf.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), sckClient);
        }

        /// <summary> 需要在外部重写 </summary>
        /// <param name="mes"> </param>
        public virtual void ReceiveAndSend(byte[] mes)
        {
            sckClient.Send(mes);
        }
    }
}