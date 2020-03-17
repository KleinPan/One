using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace EsspClassLibrary.MyNet
{
    public class Client
    {
        #region 变量

        public Socket sckClient = null;

        /// <summary> 原始字节数据 </summary>
        public byte[] ReceiveBuf = new byte[4096];

        private byte[] SendBuf = new byte[4096];

        /// <summary> 发送数据存储区 </summary>
        public string DataSend = "";

        public string Tip = "连接失败！";

        public Socket sendSocket = null;

        #endregion 变量

        /// <summary> 初始化作为客户端并连接 </summary>
        /// <param name="ip">    </param>
        /// <param name="port">  </param>
        public bool InitAsClient(string ip, int port)
        {
            try
            {
                IPAddress myIp = IPAddress.Parse(ip.Trim());

                IPEndPoint iep = new IPEndPoint(myIp, port);

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

        /// <summary> 初始化作为客户端并连接 </summary>
        /// <param name="ip">    </param>
        /// <param name="port">  </param>
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
        /// <returns>  </returns>
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
        /// <param name="ar">  </param>
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket sckConnect = (Socket)ar.AsyncState;
                sckConnect.EndConnect(ar);
                //连接成功提示
                sendSocket = sckConnect;

                Tip = "连接成功！";
            }
            catch (Exception e)
            {
                Tip = "连接异常！";
                MessageBox.Show(e.Message);
            }
            //清空接收数据缓冲区
            //Array.Clear(ReceiveBuf, 0, 256);
            Array.Clear(ReceiveBuf, 0, ReceiveBuf.Length);

            try
            {
                //接收数据
                //sckClient.BeginReceive(ReceiveBuf, 0, ReceiveBuf.Length, SocketFlags.None , new AsyncCallback(ReceiveCallback), sckClient);
                int a = sckClient.Receive(ReceiveBuf);

                Console.WriteLine(a);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}