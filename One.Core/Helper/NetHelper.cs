﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace One.Core.Helper
{
    /// <summary> 网络帮助类 </summary>
    public class NetHelper
    {
        /// <summary> 获取本机所有IPv4地址 </summary>
        public static List<IPAddress> GainIPv4AdList()
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
        public static List<IPAddress> GainIPv6AdList()
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
        /// <returns>  </returns>
        public static string GetLocalIP()
        {
            string result = RunApp("route", "print", true);
            Match m = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
            if (m.Success)
            {
                return m.Groups[2].Value;
            }
            else
            {
                try
                {
                    System.Net.Sockets.TcpClient c = new System.Net.Sockets.TcpClient();
                    c.Connect("www.baidu.com", 80);
                    string ip = ((System.Net.IPEndPoint)c.Client.LocalEndPoint).Address.ToString();
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
        /// <returns>  </returns>
        public static string GetPrimaryDNS()
        {
            string result = RunApp("nslookup", "", true);
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

        /// <summary> 运行一个控制台程序并返回其输出参数。 </summary>
        /// <param name="filename">  方法名 </param>
        /// <param name="arguments">  </param>
        /// <param name="recordLog">  </param>
        /// <returns>  </returns>
        public static string RunApp(string filename, string arguments, bool recordLog)
        {
            try
            {
                if (recordLog)
                {
                    Trace.WriteLine(filename + " " + arguments);
                }
                Process proc = new Process();
                proc.StartInfo.FileName = filename;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.Arguments = arguments;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                using (System.IO.StreamReader sr = new System.IO.StreamReader(proc.StandardOutput.BaseStream, Encoding.Default))
                {
                    //string txt = sr.ReadToEnd();
                    //sr.Close();
                    //if (recordLog)
                    //{
                    //    Trace.WriteLine(txt);
                    //}
                    //if (!proc.HasExited)
                    //{
                    //    proc.Kill();
                    //}
                    //上面标记的是原文，下面是我自己调试错误后自行修改的
                    Thread.Sleep(100);           //貌似调用系统的nslookup还未返回数据或者数据未编码完成，程序就已经跳过直接执行
                                                 //txt = sr.ReadToEnd()了，导致返回的数据为空，故睡眠令硬件反应
                    if (!proc.HasExited)         //在无参数调用nslookup后，可以继续输入命令继续操作，如果进程未停止就直接执行
                    {                            //txt = sr.ReadToEnd()程序就在等待输入，而且又无法输入，直接掐住无法继续运行
                        proc.Kill();
                    }
                    string txt = sr.ReadToEnd();
                    sr.Close();
                    if (recordLog)
                        Trace.WriteLine(txt);
                    return txt;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return ex.Message;
            }
        }

        /// <summary> 检测端口是否占用 </summary>
        /// <param name="port">  </param>
        /// <returns>  </returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;
            System.Net.NetworkInformation.IPGlobalProperties ipProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
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