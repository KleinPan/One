using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;

using One.Core.ExtensionMethods;
using One.Core.Helpers.DataProcessHelpers;
using One.Toolbox.Component;
using One.Toolbox.Helpers;
using One.Toolbox.Messenger;
using One.Toolbox.Models.Serialport;
using One.Toolbox.Services;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views;

using System.Collections.ObjectModel;

using System.IO.Ports;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace One.Toolbox.ViewModels.Serialport
{
    public partial class SerialportViewModel : BaseShowViewModel
    {
        #region SerialPortSetting

        public ObservableCollection<string> PortNameList { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedPortName;

        [ObservableProperty]
        private ObservableCollection<string> baudRateList = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedBaudRate;

        [ObservableProperty]
        private SerialportSettingViewModel serialportUISetting;

        #endregion SerialPortSetting

        [ObservableProperty]
        private string dataToSend = "";

        private byte[] toSendData = null;//待发送的数据

        private SerialPortComponent serialPortHelper { get; set; }

        /// <summary> 快捷发送列表 </summary>
        public ObservableCollection<QuickSendViewModel> QuickSendList { get; set; } = new ObservableCollection<QuickSendViewModel>();

        #region 界面显示

        [ObservableProperty]
        private string openCloseButtonContent = ResourceHelper.FindStringResource("Open");

        [ObservableProperty]
        private string statusTextBlockContent = ResourceHelper.FindStringResource("OpenPort_close");

        [ObservableProperty]
        private int sentCount;

        [ObservableProperty]
        private int receivedCount;

        [ObservableProperty]
        private bool isOpen = false;

        [ObservableProperty]
        private string logTip = "右键打开Log目录";

        #region 定时发送

        /// <summary> 定时发送 </summary>
        [ObservableProperty]
        private bool timedSend;

        [ObservableProperty]
        private int timedCount = 1000;

        #endregion 定时发送

        #endregion 界面显示

        public override void OnNavigatedLeave()
        {
            base.OnNavigatedLeave();

            SaveSetting();
        }

        public override void OnNavigatedEnter()
        {
            base.OnNavigatedEnter();
            LoadSetting();
        }

        public override void InitializeViewModel()
        {
            if (isInitialized)
                return;

            WeakReferenceMessenger.Default.Register<CloseMessage>(this, (r, m) =>
            {
                // Handle the message here, with r being the recipient and m being the input message. Using the recipient passed as input makes it so that the lambda expression doesn't capture "this", improving performance.

                SaveSetting();
            });

            RefreshPortList();

            BaudRateList.AddRange(new List<string>() { "330", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "115200", "128000", "230400" });

            SelectedBaudRate = "115200";

            RandomHeader = "Default";

            //Dtr = true;
            serialPortHelper = new SerialPortComponent();
            serialPortHelper.UartDataSent += SerialPortHelper_UartDataSent;
            serialPortHelper.UartDataRecived += SerialPortHelper_UartDataRecived;

            base.InitializeViewModel();
        }

        private void SerialPortHelper_UartDataRecived(object? sender, EventArgs e)
        {
            var data = sender as byte[];
            ReceivedCount += data.Length;

            ShowData("", data, false, SerialportUISetting.HexShow);
        }

        private void SerialPortHelper_UartDataSent(object? sender, EventArgs e)
        {
            var data1 = sender as byte[];
            SentCount += data1.Length;
            ShowData("", data: data1, send: true, SerialportUISetting.HexSend);
        }

        private bool refreshLock = false;

        #region InitUI

        [RelayCommand]
        private void InitFlowDocumentControl(object obj)
        {
            var args = obj as System.Windows.RoutedEventArgs;
            var control = args.OriginalSource as FlowDocumentScrollViewer;
            flowDocumentHelper = new FlowDocumentComponent(control);

            flowDocumentHelper.MaxPacksAutoClear = SerialportUISetting.MaxPacksAutoClear;
            flowDocumentHelper.LagAutoClear = SerialportUISetting.LagAutoClear;
        }

        #endregion InitUI

        #region Command

        [RelayCommand]
        private void ClearLog(object obj)
        {
            flowDocumentHelper.ClearContent();
        }

        [RelayCommand]
        private void MoreSerialportSetting(object obj)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.DataContext = SerialportUISetting;
            settingWindow.Show();

            SaveSetting();
        }

        /// <summary> 刷新设备列表 </summary>
        [RelayCommand]
        private void RefreshPortList(string lastPort = null)
        {
            if (refreshLock)
                return;
            refreshLock = true;
            PortNameList.Clear();
            List<string> strs = new List<string>();
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                        Regex regExp = new Regex("\\(COM\\d+\\)");
                        foreach (ManagementObject queryObj in searcher.Get())
                        {
                            if ((queryObj["Caption"] != null) && regExp.IsMatch(queryObj["Caption"].ToString()))
                            {
                                strs.Add(queryObj["Caption"].ToString());
                            }
                        }
                        break;
                    }
                    catch
                    {
                        Task.Delay(500).Wait();
                    }
                    //MessageBox.Show("fail了");
                }

                try
                {
                    foreach (string p in SerialPort.GetPortNames())//加上缺少的com口
                    {
                        //有些人遇到了微软库的bug，所以需要手动从0x00截断
                        var pp = p;
                        if (p.IndexOf("\0") > 0)
                            pp = p.Substring(0, p.IndexOf("\0"));
                        bool notMatch = true;
                        foreach (string n in strs)
                        {
                            if (n.Contains($"({pp})"))//如果和选中项目匹配
                            {
                                notMatch = false;
                                break;
                            }
                        }
                        if (notMatch)
                            strs.Add($"Serial Port {pp} ({pp})");//如果列表中没有，就自己加上
                    }
                }
                catch { }

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (var item in strs)
                    {
                        PortNameList.Add(item);
                    }

                    if (PortNameList.Count > 0)
                    {
                        SelectedPortName = PortNameList[0];
                    }
                }));

                refreshLock = false;
            });
        }

        [RelayCommand]
        private void SendData()
        {
            var data = System.Text.Encoding.UTF8.GetBytes(DataToSend);

            SendUartData(data);
        }

        /// <summary> 定时发送 </summary>
        /// <param name="value"> </param>
        /// <exception cref="NotImplementedException"> </exception>
        partial void OnTimedSendChanged(bool value)
        {
            if (value)
            {
                Task.Run(() =>
                {
                    while (TimedSend)
                    {
                        var data = System.Text.Encoding.UTF8.GetBytes(DataToSend);

                        SendUartData(data);

                        Thread.Sleep(TimedCount);
                    }
                });
            }
        }

        [RelayCommand]
        private void AddQuickSendItem()
        {
            QuickSendList.Add(new QuickSendViewModel() { Id = QuickSendList.Count + 1, Text = "", Hex = false, Commit = ResourceHelper.FindStringResource("QuickSendButton") });
        }

        private bool forcusClosePort = true;

        [RelayCommand]
        private void OpenClosePort()
        {
            if (!serialPortHelper.IsOpen())//打开串口逻辑
            {
                OpenPort();
            }
            else//关闭串口逻辑
            {
                string lastPort = null;//记录一下上次的串口号
                try
                {
                    forcusClosePort = true;//不再重新开启串口

                    serialPortHelper.Close();
                }
                catch
                {
                    //串口关闭失败！

                    MessageShowHelper.ShowErrorMessage(ResourceHelper.FindStringResource("ErrorClosePort"));
                }

                OpenCloseButtonContent = ResourceHelper.FindStringResource("Open");

                IsOpen = false;

                StatusTextBlockContent = ResourceHelper.FindStringResource("OpenPort_close");

                //refreshPortList(lastPort);
            }
        }

        [RelayCommand]
        private void SaveLog()
        {
            GenerateRandomHeader();
        }

        #endregion Command

        /// <summary> 是否正在打开端口 </summary>
        private bool isOpeningPort = false;

        private void OpenPort()
        {
            if (isOpeningPort)
                return;
            if (SelectedPortName != null)
            {
                string[] ports;//获取所有串口列表
                try
                {
                    WriteTraceLog($"GetPortNames");
                    ports = SerialPort.GetPortNames();
                    WriteTraceLog($"GetPortNames{ports.Length}");
                }
                catch (Exception e)
                {
                    ports = new string[0];
                    NLogger.Error($"[openPort]GetPortNames Exception:{e.Message}");
                }
                string port = "";//最终串口名
                foreach (string p in ports)//循环查找符合名称串口
                {
                    //有些人遇到了微软库的bug，所以需要手动从0x00截断
                    var pp = p;
                    if (p.IndexOf("\0") > 0)
                        pp = p.Substring(0, p.IndexOf("\0"));
                    if ((SelectedPortName as string).Contains($"({pp})"))//如果和选中项目匹配
                    {
                        port = pp;
                        break;
                    }
                }

                if (port != "")
                {
                    Task.Run(() =>
                    {
                        isOpeningPort = true;
                        try
                        {
                            forcusClosePort = false;//不再强制关闭串口
                            WriteTraceLog($"SetName");
                            serialPortHelper.SetName(port);

                            serialportUISetting.SerialportParams.BaudRate = int.Parse(SelectedBaudRate);
                            WriteTraceLog($"Open");

                            serialPortHelper.Open(serialportUISetting);

                            IsOpen = true;
                            App.Current.Dispatcher.Invoke(new Action(delegate
                            {
                                OpenCloseButtonContent = ResourceHelper.FindStringResource("OpenPort_close");

                                //serialPortsListComboBox.IsEnabled = false;
                                StatusTextBlockContent = ResourceHelper.FindStringResource("Open");
                            }));

                            if (toSendData != null)
                            {
                                SendUartData(toSendData);
                                toSendData = null;
                            }
                        }
                        catch (Exception e)
                        {
                            //串口打开失败！
                            MessageShowHelper.ShowErrorMessage(ResourceHelper.FindStringResource("ErrorOpenPort"));
                        }
                        isOpeningPort = false;
                    });
                }
            }
        }

        /// <summary> 发串口数据 </summary>
        /// <param name="data"> </param>
        private void SendUartData(byte[] data)
        {
            if (serialPortHelper.IsOpen())
            {
                byte[] dataConvert;

                try
                {
                    if (SerialportUISetting.HexSend)
                    {
                        var temp = System.Text.Encoding.UTF8.GetString(data.ToArray());

                        var temp2 = temp.Replace(" ", "").Replace("\r\n", "");
                        dataConvert = StringHelper.HexStringToBytes(temp);
                    }
                    else
                    {
                        dataConvert = data;
                    }

                    if (SerialportUISetting.WithExtraEnter)
                    {
                        var temp = dataConvert.ToList();
                        temp.Add(0x0d);
                        temp.Add(0x0a);
                        dataConvert = temp.ToArray();
                    }
                    serialPortHelper.SendData(dataConvert);
                }
                catch (Exception ex)
                {
                    MessageShowHelper.ShowErrorMessage($"{ResourceHelper.FindStringResource("ErrorSendFail")}\r\n" + ex.ToString());
                    return;
                }
            }
        }

        #region Setting

        public void SaveSetting()
        {
            var service = App.Current.Services.GetService<SettingService>();

            service.AllConfig.SerialportSetting = SerialportUISetting.ToModel();
            service.AllConfig.SerialportParams = SerialportUISetting.SerialportParams;

            service.Save();
        }

        public void LoadSetting()
        {
            var service = App.Current.Services.GetService<SettingService>();

            SerialportUISetting = service.AllConfig.SerialportSetting.ToVM();

            SerialportUISetting.SerialportParams = service.AllConfig.SerialportParams;

            QuickSendList.Clear();
            QuickSendList.AddRange(SerialportUISetting.QuickSendList);
        }

        #endregion Setting

        #region Log

        public string RandomHeader { get; set; }

        private void GenerateRandomHeader()
        {
            Random random = new Random();
            RandomHeader = random.Next(0, 100000).ToString();

            LogTip = "右键打开Log目录,前缀-> " + RandomHeader;
        }

        public override void WriteInfoLog(string msg)
        {
            NLogger.WithProperty("Random", RandomHeader).Info(msg);
        }

        #endregion Log
    }
}