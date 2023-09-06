using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace One.Core.Helpers.NetHelpers
{
    public class ClientHelper : BaseHelper
    {
        #region 变量

        private Socket sckClient = null;

        /// <summary> 原始字节数据 </summary>
        private byte[] receiveBuffer = new byte[1024];

        /// <summary> 发送数据存储区 </summary>
        public string DataSend = "";

        private Socket sendSocket = null;

        #endregion 变量

        public Action<byte[]> ReceiveAction;

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

                //int revLength = sckReceive.EndReceive(ar);
            }
            catch (Exception e)
            {
                WriteLog(e.ToString());
            }
            //清空接收数据缓冲区
            //Array.Clear(ReceiveBuf, 0, 256);
            Array.Clear(receiveBuffer, 0, receiveBuffer.Length);

            try
            {
                //接收数据
                sckClient.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), sckClient);
                //int a = sckClient.Receive(ReceiveBuf);

                //ReceiveAction?.Invoke(receiveBuffer.Take(revLength).ToArray());
                //ReceiveAction?.Invoke(receiveBuffer);
            }
            catch (Exception e)
            {
                WriteLog(e.ToString());
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

        public void SendData(byte[] data)
        {
            try
            {
                int count = sckClient.Send(data);
                Console.WriteLine("发送数据长度为：" + count);
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
                Array.Clear(receiveBuffer, 0, receiveBuffer.Length);

                int count = sckClient.Receive(receiveBuffer);
                Console.WriteLine("接收数据长度为：" + count);
                return receiveBuffer;
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

            //再次接收数据
            sckClient.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), sckClient);

            ReceiveAction?.Invoke(receiveBuffer.Take(revLength).ToArray());
        }
    }
}