﻿using One.Core.Helpers.DataProcessHelpers;
using One.Core.Helpers.NetHelpers;
using One.Toolbox.Component;
using One.Toolbox.Enums;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;

using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;

namespace One.Toolbox.ViewModels.Network;

public partial class NetworkPageVM : BaseShowViewModel
{
    [ObservableProperty]
    private CommunProtocalType selectCommunProtocalType;

    [ObservableProperty]
    private bool isConnected = false;

    //是否可以进行操作，例如连接或者断开,监听或者停止监听
    [ObservableProperty]
    private bool changeable = true;

    [ObservableProperty]
    private bool isListening = false;

    [ObservableProperty]
    private bool hexMode = false;

    [ObservableProperty]
    private ObservableCollection<string> ipList = new ObservableCollection<string>();

    [ObservableProperty]
    private ObservableCollection<string> remoteIpList = new ObservableCollection<string>();

    [ObservableProperty]
    private string selectedIP;

    [ObservableProperty]
    private string remoteSelectedIP;

    [ObservableProperty]
    private string inputPort = "2333";

    private AsyncTCPClient asyncTCPClient;
    private AsyncTCPServer asyncTCPServer;

    private AsyncUDPClient asyncUDPClient;

    public override void InitializeViewModel()
    {
        if (isInitialized)
            return;

        RefreshIp();

        asyncTCPClient = new AsyncTCPClient(WriteDebugLog);
        asyncTCPClient.ReceiveAction += ShowReceiveAction;
        asyncTCPClient.SendAction += ShowSendMessage;
        asyncTCPClient.OnConnected += ShowInfoAction;
        asyncTCPClient.OnDisConnected += ShowInfoAction;

        asyncTCPServer = new AsyncTCPServer(WriteDebugLog);
        asyncTCPServer.ReceiveAction += ShowServerReceiveAction;
        asyncTCPServer.SendAction += ShowSendMessage;
        asyncTCPServer.OnConnected += ShowInfoAction;
        asyncTCPServer.OnDisConnected += ShowInfoAction;
        asyncTCPServer.OnIpEndPointChanged += IpEndPointChangedEvent;

        asyncUDPClient = new AsyncUDPClient(WriteDebugLog);
        asyncUDPClient.ReceiveAction += ShowServerReceiveAction;
        asyncUDPClient.SendAction += ShowSendMessage;
        asyncUDPClient.OnConnected += ShowInfoAction;
        asyncUDPClient.OnDisConnected += ShowInfoAction;
        asyncUDPClient.OnIpEndPointChanged += IpEndPointChangedEvent;
        base.InitializeViewModel();
    }

    private void IpEndPointChangedEvent(IPListOperationEnum operation, string arg2)
    {
        switch (operation)
        {
            case IPListOperationEnum.Add:

                if (!RemoteIpList.Contains(arg2))
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        RemoteIpList.Add(arg2);
                    });
                }

                break;

            case IPListOperationEnum.Subtract:

                if (RemoteIpList.Contains(arg2))
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        RemoteIpList.Remove(arg2);
                    });
                }

                break;

            default:
                break;
        }
    }

    private void ShowReceiveAction(byte[] data)
    {
        ShowData("", data, false);
    }

    private void ShowSendMessage(byte[] data)
    {
        ShowData("", data, true);
    }

    private void ShowInfoAction(byte[] data)
    {
        var msg = System.Text.Encoding.UTF8.GetString(data);

        ShowData(msg, null, true);
    }

    private void ShowServerReceiveAction(string title, byte[] data)
    {
        ShowData(title, data, false);
    }

    [ObservableProperty]
    private string dataToSend;

    #region InitUI

    [RelayCommand]
    private void InitFlowDocumentControl(object obj)
    {
        var args = obj as System.Windows.RoutedEventArgs;
        var control = args.OriginalSource as FlowDocumentScrollViewer;
        flowDocumentHelper = new FlowDocumentComponent(control);
    }

    #endregion InitUI

    #region Command

    [RelayCommand]
    private void ClearRemoteIPList()
    {
        RemoteIpList.Clear();
    }

    [RelayCommand]
    private void Listen()
    {
        IPAddress ip = null;
        try
        {
            ip = IPAddress.Parse(SelectedIP);

            switch (SelectCommunProtocalType)
            {
                case CommunProtocalType.TCP_Client:
                    break;

                case CommunProtocalType.TCP_Server:
                    asyncTCPServer.InitAsServer(ip, int.Parse(InputPort));

                    break;

                case CommunProtocalType.UDP_Client:

                    asyncUDPClient.InitClient(ip, int.Parse(InputPort));
                    break;

                case CommunProtocalType.Test:
                    break;

                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            MessageShowHelper.ShowErrorMessage(ex.ToString());
            return;
        }

        Changeable = false;
        IsListening = true;
    }

    [RelayCommand]
    private void StopListen()
    {
        try
        {
            switch (SelectCommunProtocalType)
            {
                case CommunProtocalType.TCP_Client:
                    break;

                case CommunProtocalType.TCP_Server:

                    asyncTCPServer.ReleaseServer();
                    break;

                case CommunProtocalType.UDP_Client:
                    {
                        asyncUDPClient.ReleaseClient();
                    }
                    break;

                case CommunProtocalType.Test:
                    break;

                default:
                    break;
            }

            IsListening = false;
            Changeable = true;
        }
        catch (Exception ex)
        {
            MessageShowHelper.ShowErrorMessage(ex.ToString());
        }
    }

    #endregion Command

    #region 通用功能

    /// <summary> 刷新本机ip列表 </summary>
    [RelayCommand]
    private void RefreshIp()
    {
        IpList.Clear();
        IpList.Add("0.0.0.0");
        IpList.Add("127.0.0.1");
        //IpList.Add("::");
        var temp = new List<string>();
        try
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)//ipa.AddressFamily == AddressFamily.InterNetworkV6
                    temp.Add(ipa.ToString());
            }
        }
        catch { }
        //去重
        temp.Distinct().ToList().ForEach(ip => IpList.Add(ip));
    }

    [RelayCommand]
    private void SocketConnect()
    {
        if (!Changeable)
            return;

        try
        {
            Changeable = false;
            IPAddress ip = null;
            try
            {
                ip = IPAddress.Parse(SelectedIP);
            }
            catch
            {
                var hostEntry = Dns.GetHostEntry(SelectedIP);
                ip = hostEntry.AddressList[0];
            }
            var ipe = new IPEndPoint(ip, int.Parse(InputPort));

            switch (SelectCommunProtocalType)
            {
                case CommunProtocalType.TCP_Client:

                    //s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    asyncTCPClient.InitClient(ip, int.Parse(InputPort));
                    break;

                case CommunProtocalType.TCP_Server:

                    break;

                case CommunProtocalType.UDP_Client:

                    break;

                case CommunProtocalType.Test:
                    break;

                default:
                    break;
            }

            IsConnected = true;
        }
        catch (Exception ex)
        {
            MessageShowHelper.ShowErrorMessage($"Server information error {ex.Message}");
            Changeable = true;
            return;
        }
    }

    [RelayCommand]
    private void SocketDisconnect()
    {
        switch (SelectCommunProtocalType)
        {
            case CommunProtocalType.TCP_Client:
                {
                    asyncTCPClient.ReleaseClient();
                }

                break;

            case CommunProtocalType.TCP_Server:
                break;

            case CommunProtocalType.UDP_Client:
                {
                }
                break;

            case CommunProtocalType.Test:
                break;

            default:
                break;
        }

        IsConnected = false;
        Changeable = true;
    }

    [RelayCommand]
    private void SendData(object toAll)
    {
        if (DataToSend == null)
        {
            return;
        }

        byte[] buff = HexMode ? StringHelper.HexStringToBytes(DataToSend) ://ByteHelper.HexToByte(DataToSend)
                      System.Text.Encoding.UTF8.GetBytes(DataToSend); ;

        switch (SelectCommunProtocalType)
        {
            case CommunProtocalType.TCP_Client:
                {
                    asyncTCPClient.SendData(buff);
                }

                break;

            case CommunProtocalType.TCP_Server:
                {
                    if (toAll != null)
                    {
                        var param = toAll.ToString();
                        if (param == "ToAll")
                        {
                            asyncTCPServer.SendDataToAll(buff);
                        }
                    }
                    else
                    {
                        var temp = RemoteSelectedIP.Replace(" ", "");
                        IPEndPoint iPEndPoint = IPEndPoint.Parse(temp);

                        asyncTCPServer.SendData(buff, iPEndPoint);
                    }
                }
                break;

            case CommunProtocalType.UDP_Client:
                {
                    try
                    {
                        //RemoteSelectedIP = "8.8.8.8:8799";

                        var temp = RemoteSelectedIP.Replace(" ", "");
                        IPEndPoint iPEndPoint = IPEndPoint.Parse(temp);
                        asyncUDPClient.SendData(iPEndPoint, buff);
                    }
                    catch (Exception ex)
                    {
                        MessageShowHelper.ShowErrorMessage(ex.ToString());
                    }
                }
                break;

            case CommunProtocalType.Test:
                break;

            default:
                break;
        }
    }

    #endregion 通用功能

    /// <summary> 获取客户端的名字 </summary>
    /// <param name="s"> </param>
    /// <returns> </returns>
    private string GetClientName(Socket s)
    {
        var remote = (IPEndPoint)s.RemoteEndPoint;
        var remoteIsV6 = remote.Address.ToString().Contains(":");
        var local = (IPEndPoint)s.LocalEndPoint;
        var localIsV6 = local.Address.ToString().Contains(":");
        return $"{(remoteIsV6 ? "[" : "")}{remote.Address}{(remoteIsV6 ? "]" : "")}:{remote.Port} → " +
            $"{(localIsV6 ? "[" : "")}{local.Address}{(localIsV6 ? "]" : "")}:{local.Port}";
    }
}