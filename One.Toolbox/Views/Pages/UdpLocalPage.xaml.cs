﻿using One.Toolbox.ViewModels;

using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;

namespace One.Toolbox.Views.Pages
{
    /// <summary> UdpLocalPage.xaml 的交互逻辑 </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class UdpLocalPage
    {
        public UdpLocalPage()
        {
            InitializeComponent();
        }

        public bool IsConnected { get; set; } = false;

        private static bool loaded = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded)
                return;
            loaded = true;
            RefreshIp();
            //绑定
            MainGrid.DataContext = this;
            IpPortTextBox.DataContext = Tools.Global.setting;
        }

        /// <summary> 刷新本机ip列表 </summary>
        private void RefreshIp()
        {
            IpListComboBox.Items.Clear();
            IpListComboBox.Items.Add("0.0.0.0");
            IpListComboBox.Items.Add("::");
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
            temp.Distinct().ToList().ForEach(ip => IpListComboBox.Items.Add(ip));
            IpListComboBox.SelectedIndex = 0;
        }

        private void ShowData(string title, byte[] data = null, bool send = false)
        {
            //Tools.Logger.ShowDataRaw(new Tools.DataShowRaw
            //{
            //    title = $"🗑 local udp server: {title}",
            //    data = data ?? new byte[0],
            //    color = send ? Brushes.DarkRed : Brushes.DarkGreen,
            //});
        }

        private UdpClient Server = null;

        /// <summary> 开始监听服务器 </summary>
        /// <param name="ip">   </param>
        /// <param name="port"> </param>
        /// <returns> </returns>
        private bool StartServer(string ip, int port)
        {
            if (Server != null)
                return false;
            IPAddress localAddr = IPAddress.Parse(ip);
            IPEndPoint IpEndPoint = new IPEndPoint(localAddr, port);
            Server = new UdpClient(IpEndPoint);

            var isV6 = ip.Contains(":");
            ShowData($"🗑 {(isV6 ? "[" : "")}{ip}{(isV6 ? "]" : "")}:{port}");

            AsyncCallback newConnectionCb = null;
            newConnectionCb = new AsyncCallback((ar) =>
            {
                try
                {
                    UdpClient u = ((UdpState)(ar.AsyncState)).u;
                    IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

                    byte[] receiveBytes = u.EndReceive(ar, ref e);
                    var isV6 = e.Address.ToString().Contains(":");
                    ShowData($"{(isV6 ? "[" : "")}{e.Address}{(isV6 ? "]" : "")}:{e.Port}", receiveBytes);
                    Server.BeginReceive(newConnectionCb, ar.AsyncState);
                }
                catch { }
            });
            UdpState s = new UdpState();
            s.e = IpEndPoint;
            s.u = Server;
            try
            {
                Server.BeginReceive(newConnectionCb, s);
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
            Server?.Close();
            Server?.Dispose();
            Server = null;
        }

        private void RefreshIpButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshIp();
        }

        private void ListenButton_Click(object sender, RoutedEventArgs e)
        {
            int port;
            if (int.TryParse(IpPortTextBox.Text, out port))
            {
                try
                {
                    IsConnected = StartServer(IpListComboBox.Text, port);
                }
                catch (Exception err)
                {
                    Tools.MessageBox.Show(err.Message);
                }
            }
        }

        private void StopListenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StopServer();
                IsConnected = false;
                ShowData($"🚫 server closed");
            }
            catch { }
        }
    }

    public struct UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }
}