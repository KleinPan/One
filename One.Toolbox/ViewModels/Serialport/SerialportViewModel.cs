using One.Core.ExtensionMethods;
using One.Core.Helpers.DataProcessHelpers;
using One.Toolbox.Component;
using One.Toolbox.Helpers;
using One.Toolbox.Model;
using One.Toolbox.Models;
using One.Toolbox.Tools;
using One.Toolbox.Views;

using System.Collections.ObjectModel;

using System.IO.Ports;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;

namespace One.Toolbox.ViewModels.Serialport
{
    public partial class SerialportViewModel : BaseViewModel
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
        public ObservableCollection<ToSendData> QuickSendList { get; set; } = new ObservableCollection<ToSendData>();

        #region 界面显示

        [ObservableProperty]
        private string openCloseButtonContent = ResourceHelper.FindStringResource("OpenPort_open");

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

            ShowData(data, false, SerialportUISetting.HexShow);
        }

        private void SerialPortHelper_UartDataSent(object? sender, EventArgs e)
        {
            var data = sender as byte[];
            SentCount += data.Length;
            ShowData(data, true, SerialportUISetting.HexSend);
        }

        private void ShowData(byte[] data, bool send, bool hexMode)
        {
            string realData;
            if (hexMode)
            {
                realData = StringHelper.BytesToHexString(data);
            }
            else
            {
                realData = System.Text.Encoding.UTF8.GetString(data);
            }

            DataShowCommon dataShowCommon = new DataShowCommon()
            {
                Send = send,
                Message = realData,
                MessageColor = send ? Brushes.DarkRed : Brushes.DarkGreen,
                Prefix = send ? " << " : " >> ",
            };
            flowDocumentHelper.DataShowAdd(dataShowCommon);

            WriteInfoLog(realData);
        }

        private bool refreshLock = false;

        #region InitUI

        private FlowDocumentComponent flowDocumentHelper { get; set; }

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
            SendUartData(Global.GetEncoding().GetBytes(DataToSend));
        }

        [RelayCommand]
        private void AddQuickSendItem()
        {
            QuickSendList.Add(new ToSendData() { Id = QuickSendList.Count + 1, Text = "", Hex = false, Commit = ResourceHelper.FindStringResource("QuickSendButton") });
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

                OpenCloseButtonContent = ResourceHelper.FindStringResource("OpenPort_open");

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
                                StatusTextBlockContent = ResourceHelper.FindStringResource("OpenPort_open");
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
                    //dataConvert = SerialportUISetting.HexSend ? ByteHelper.HexToByte(ByteHelper.ByteToString(data)) : data;
                    dataConvert = SerialportUISetting.HexSend ? StringHelper.HexStringToBytes(StringHelper.BytesToHexString(data)) : data;

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
            if (serialportUISetting == null)
            {
                serialportUISetting = new SerialportSettingViewModel();
            }

            ConfigHelper.Instance.AllConfig.SerialportSetting = SerialportUISetting.ToModel();
            ConfigHelper.Instance.AllConfig.SerialportParams = SerialportUISetting.SerialportParams;

            ConfigHelper.Instance.Save();
        }

        public void LoadSetting()
        {
            SerialportUISetting = ConfigHelper.Instance.AllConfig.SerialportSetting.ToVM();

            SerialportUISetting.SerialportParams = ConfigHelper.Instance.AllConfig.SerialportParams;

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