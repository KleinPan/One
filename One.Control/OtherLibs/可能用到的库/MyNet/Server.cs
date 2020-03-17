using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace EsspClassLibrary.MyNet
{
    public class Server
    {
        #region 变量

        /// <summary>服务器套接字</summary>
        private Socket sckServer = null;

        public Socket sckSend = null;

        public ObservableCollection<Socket> listConnection = new ObservableCollection<Socket>();

        /// <summary>接收缓冲区</summary>
        private byte[] ReceiveBuf = new byte[1024 * 10];

        /// <summary>发送缓冲区</summary>
        private byte[] SendBuf = new byte[1024 * 10];

        /// <summary>接收数据存储区</summary>
        public string DataReceived = "";

        #endregion 变量

        /// <summary>初始化作为服务器并启动</summary>
        /// <param name="ip">  </param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool InitAsServer(string ip, int port)
        {
            try
            {
                IPAddress myIp = IPAddress.Parse(ip);
                IPEndPoint iep = new IPEndPoint(myIp, port);

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
                Console.WriteLine("等待接受连接请求！");

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        //注意
        //需要重新写服务器,多个客户端连接要记录服务器的socket

        /// <summary>接受连接请求回调函数—— 连接完成后执行</summary>
        /// <param name="ar"></param>
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
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    listConnection.Add(sckAccept);
                }), null);

                sckSend = sckAccept;//接收和发送用的是一个套接字（连接）
                SendData("Connected!");
                //Console.WriteLine("连接成功！");
                //清空接收数据缓冲区
                Array.Clear(ReceiveBuf, 0, 255);

                //接收数据
                sckSend.BeginReceive(ReceiveBuf, 0, 256, SocketFlags.None, new AsyncCallback(ServeReceivedCallback), sckSend);

                //接受连接请求
                sckServer.BeginAccept(new AsyncCallback(ConnectedCallback), sckServer);
            }
            catch(Exception e)
            {
                //string s = e.Message.Replace("\r\n", "\t");
                //tsslShow.Text = s + "服务器关闭！";
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>接收数据回调函数—— 接收完显示用</summary>
        /// <param name="ar"></param>
        private void ServeReceivedCallback(IAsyncResult ar)
        {
            try
            {
                Socket sckReceive = (Socket)ar.AsyncState;
                int revLength = sckReceive.EndReceive(ar);

                //把接收到的数据转成字符串显示到界面
                string strReceive = Encoding.UTF8.GetString(ReceiveBuf, 0, revLength);
                DataReceived = strReceive;

                Console.WriteLine(DataReceived);
                ReceiveAndSend(DataReceived);
                //再次接收数据
                sckReceive.BeginReceive(ReceiveBuf, 0, 256, SocketFlags.None, new AsyncCallback(ServeReceivedCallback), sckReceive);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SeverEndConnectCallback(IAsyncResult ar)
        {
            try
            {
                sckServer.EndDisconnect(ar);
                listConnection.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>发送数据</summary>
        /// <param name="data"></param>
        public void SendData(string data)
        {
            SendBuf = Encoding.UTF8.GetBytes(data);

            sckSend.BeginSend(SendBuf, 0, SendBuf.Length, SocketFlags.None, new AsyncCallback(SendCallback), sckSend);
        }

        /// <summary>发送数据回调函数</summary>
        /// <param name="ar"></param>
        private void SendCallback(IAsyncResult ar)
        {
            Socket sckSend = (Socket)ar.AsyncState;
            int sendLen = sckSend.EndSend(ar);
        }

        /// <summary>需要在外部重写对消息格式 最后需要加上SendData(string data)</summary>
        /// <param name="mes"></param>
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
    }
}