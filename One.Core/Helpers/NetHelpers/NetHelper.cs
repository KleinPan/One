using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace One.Core.Helpers.NetHelpers
{
    /// <summary> 网络帮助类 </summary>
    public class NetHelper
    {
        /// <summary> 获取本机所有IPv4地址 </summary>
        public static List<IPAddress> GetIPv4AdList()
        {
            string hostName = Dns.GetHostName();//本机名
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6
            List<IPAddress> address4List = new List<IPAddress>();
            foreach (IPAddress ipa in addressList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    //LocalIP = ipa;
                    //return ipa.ToString();
                    address4List.Add(ipa);
                    //return ipa;
                }
            }
            return address4List;
        }

        /// <summary> 获取本机所有IPv6地址 </summary>
        public static List<IPAddress> GetIPv6AdList()
        {
            string hostName = Dns.GetHostName();//本机名
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6
            List<IPAddress> address4List = new List<IPAddress>();
            foreach (IPAddress ipa in addressList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    //LocalIP = ipa;
                    //return ipa.ToString();
                    address4List.Add(ipa);
                    //return ipa;
                }
            }
            return address4List;
        }

        /// <summary> 获取当前使用的IP,Ping baidu </summary>
        /// <returns> </returns>
        public static string GetLocalIP()
        {
            string result;
            new ProcessHelper(null).RunExeAndReadResult("", "route", "print", out result);
            Match m = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
            if (m.Success)
            {
                return m.Groups[2].Value;
            }
            else
            {
                try
                {
                    TcpClient c = new TcpClient();
                    c.Connect("www.baidu.com", 80);
                    string ip = ((IPEndPoint)c.Client.LocalEndPoint).Address.ToString();
                    c.Close();
                    return ip;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary> 获取本机主DNS </summary>
        /// <returns> </returns>
        public static string GetPrimaryDNS()
        {
            new ProcessHelper(null).RunExeAndReadResult("", "nslookup", "", out string result);
            Match m = Regex.Match(result, @"\d+\.\d+\.\d+\.\d+");
            if (m.Success)
            {
                return m.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary> 检测端口是否占用 </summary>
        /// <param name="port"> </param>
        /// <returns> </returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        /// <summary> 获取本机网络信息 </summary>
        public static List<NetWorkInfo> GetLocalNetInfo()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            List<NetWorkInfo> listNetWorkInfo = new List<NetWorkInfo>();

            foreach (NetworkInterface bendi in interfaces)
            {
                if ((bendi.NetworkInterfaceType.ToString().Equals("Ethernet") || bendi.NetworkInterfaceType.ToString().Equals("Wireless80211")) && bendi.OperationalStatus == OperationalStatus.Up)
                {
                    NetWorkInfo netWorkInfo = new NetWorkInfo();

                    IPInterfaceProperties ip = bendi.GetIPProperties();
                    //获取Ip 掩码
                    for (int i = 0; i < ip.UnicastAddresses.Count; i++)
                    {
                        //不插网线会得到一个保留地址 169.254.126.164
                        if (ip.UnicastAddresses[i].Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (ip.UnicastAddresses[i].Address != null)
                            {
                                //MessageBox.Show(ip.UnicastAddresses[i].Address.ToString());
                                Console.WriteLine(ip.UnicastAddresses[i].Address.ToString());
                                netWorkInfo.IP = ip.UnicastAddresses[i].Address;
                            }

                            //如果不插网线 获取不了掩码 返回null
                            if (ip.UnicastAddresses[i].IPv4Mask != null)
                            {
                                //MessageBox.Show(ip.UnicastAddresses[i].IPv4Mask.ToString());
                                Console.WriteLine(ip.UnicastAddresses[i].IPv4Mask.ToString());
                                netWorkInfo.IPv4Mask = ip.UnicastAddresses[i].IPv4Mask;
                            }
                        }
                    }
                    //获取网关
                    if (ip.GatewayAddresses.Count > 0)
                    {
                        //MessageBox.Show(ip.GatewayAddresses[0].Address.ToString());
                        Console.WriteLine(ip.GatewayAddresses[0].Address.ToString());
                        netWorkInfo.GatewayAddresses.Add(ip.GatewayAddresses[0].Address);
                    }

                    //获取DNS
                    //不要DnsAddresses[0].Address.ToString() 不正确 还有警告  “System.Net.IPAddress.Address”已过时:
                    if (ip.DnsAddresses.Count > 0)
                    {
                        //MessageBox.Show(ip.DnsAddresses[0].ToString());
                        Console.WriteLine(ip.DnsAddresses[0].ToString());
                        netWorkInfo.DnsAddresses.Add(ip.DnsAddresses[0]);
                    }

                    //备用DNS
                    if (ip.DnsAddresses.Count > 1)
                    {
                        //MessageBox.Show(ip.DnsAddresses[1].ToString());
                        Console.WriteLine(ip.DnsAddresses[1].ToString());
                        netWorkInfo.DnsAddresses.Add(ip.DnsAddresses[1]);
                    }
                    listNetWorkInfo.Add(netWorkInfo);
                }
            }

            return listNetWorkInfo;
        }

        /// <summary> 检测网络是否连通 </summary>
        public static bool Ping(string hostName)
        {
            bool online = false; //是否在线
            Ping ping = new Ping();

            PingReply pingReply = ping.Send(hostName);
            if (pingReply.Status == IPStatus.Success)
            {
                online = true;
                Console.WriteLine("当前在线，已ping通！");
            }
            else
            {
                Console.WriteLine("不在线，ping不通！");
            }

            return online;
        }
    }

    public class NetWorkInfo
    {
        public NetWorkInfo()
        {
            GatewayAddresses = new List<IPAddress>();
            DnsAddresses = new List<IPAddress>();
        }

        /// <summary> IP地址 </summary>
        public IPAddress IP { get; set; }

        /// <summary> 子网掩码 </summary>
        public IPAddress IPv4Mask { get; set; }

        /// <summary> 网关 </summary>
        public List<IPAddress> GatewayAddresses { get; set; }

        /// <summary> DNS </summary>
        public List<IPAddress> DnsAddresses { get; set; }

        /// <summary> 广播地址 </summary>
        public IPAddress BroadcastPoint
        {
            get { return GetBroadcastPoint(); }
        }

        private IPAddress GetBroadcastPoint()
        {
            if (IP != null && IPv4Mask != null)
            {
                byte[] a = IP.GetAddressBytes();
                byte[] b = IPv4Mask.GetAddressBytes();

                byte[] c = new byte[a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    int d = a[i] & b[i];
                    c[i] = (byte)d;
                }
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == 0)
                    {
                        c[i] = 255;
                    }
                }

                string e = c[0] + "." + c[1] + "." + c[2] + "." + c[3];

                IPAddress iPAddress = IPAddress.Parse(e);
                return iPAddress;
            }
            else
            {
                return null;
            }
        }
    }
}