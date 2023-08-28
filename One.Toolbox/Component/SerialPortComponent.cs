using One.Toolbox.ViewModels;
using One.Toolbox.ViewModels.Serialport;

using System.IO;
using System.IO.Ports;

namespace One.Toolbox.Component
{
    internal class SerialPortComponent : BaseViewModel
    {
        //废弃的串口对象，存放处，尝试fix[System.ObjectDisposedException: 已关闭 Safe handle]
        //https://drdump.com/Problem.aspx?ProblemID=524533
        private List<SerialPort> useless = new List<SerialPort>();

        public SerialPort serialPort = new SerialPort();

        /// <summary> 收到数据的回调 </summary>
        public event EventHandler UartDataRecived;

        public event EventHandler UartDataSent;

        private Stream lastPortBaseStream = null;

        private bool _dtr = true;

        private static readonly object objLock = new object();

        private SerialportSettingViewModel SerialportSetting { get; set; }

        /// <summary> 初始化串口各个触发函数 </summary>
        public SerialPortComponent()
        {
            //声明接收到事件
            serialPort.DataReceived += Serial_DataReceived;
            //serialPort.RtsEnable = Rts;
            //serialPort.DtrEnable = Dtr;
            new Thread(ReadData).Start();

            //适配一下通用通道
            /*
            LuaApis.SendChannelsRegister("uart", (data, _) =>
            {
                if (IsOpen() && data != null)
                {
                    SendData(data);
                    return true;
                }
                else
                    return false;
            });
            */
        }

        /// <summary> 刷新串口对象 </summary>
        private void refreshSerialDevice()
        {
            NLogger.Info($"[refreshSerialDevice]start");

            try
            {
                NLogger.Info($"[refreshSerialDevice]lastPortBaseStream.Dispose");
                Task.Run(() =>//这行代码会卡住，我扔task里还卡吗？
                {
                    try
                    {
                        lastPortBaseStream?.Dispose();
                    }
                    catch { }
                });
            }
            catch (Exception e)
            {
                NLogger.Error($"[refreshSerialDevice]lastPortBaseStream.Dispose error:{e.Message}");
                Console.WriteLine($"portBaseStream?.Dispose error:{e.Message}");
            }
            try
            {
                NLogger.Info($"[refreshSerialDevice]BaseStream.Dispose");
                Task.Run(() =>//这行代码会卡住，我扔task里还卡吗？
                {
                    try
                    {
                        serialPort.BaseStream.Dispose();
                    }
                    catch { }
                });
            }
            catch (Exception e)
            {
                NLogger.Error($"[refreshSerialDevice]BaseStream.Dispose error:{e.Message}");
                Console.WriteLine($"serial.BaseStream.Dispose error:{e.Message}");
            }
            NLogger.Info($"[refreshSerialDevice]Dispose");
            Task.Run(() =>//我服了
            {
                try
                {
                    serialPort.Dispose();
                }
                catch { }
            });
            NLogger.Info($"[refreshSerialDevice]new");
            lock (useless)//存起来
                useless.Add(serialPort);
            serialPort = new SerialPort();
            //声明接收到事件
            serialPort.DataReceived += Serial_DataReceived;
            //serialPort.BaudRate = Tools.Global.setting.baudRate;
            //serialPort.Parity = (Parity)Tools.Global.setting.parity;
            //serialPort.DataBits = Tools.Global.setting.dataBits;
            //serialPort.StopBits = (StopBits)Tools.Global.setting.stopBit;
            //serialPort.RtsEnable = Rts;
            //serialPort.DtrEnable = Dtr;
            NLogger.Info($"[refreshSerialDevice]done");
        }

        /// <summary> 获取串口设备COM名 </summary>
        /// <returns> </returns>
        public string GetName()
        {
            return serialPort.PortName;
        }

        /// <summary> 设置串口设备COM名 </summary>
        /// <returns> </returns>
        public void SetName(string s)
        {
            serialPort.PortName = s;
        }

        /// <summary> 查看串口打开状态 </summary>
        /// <returns> </returns>
        public bool IsOpen()
        {
            return serialPort.IsOpen;
        }

        /// <summary> 开启串口 </summary>
        public void Open(SerialportSettingViewModel serialportParams)
        {
            string temp = serialPort.PortName;
            NLogger.Info($"[UartOpen]refreshSerialDevice");
            refreshSerialDevice();
            serialPort.PortName = temp;

            this.SerialportSetting = serialportParams;

            serialPort.BaudRate = SerialportSetting.SerialportParams.BaudRate;
            serialPort.Parity = (Parity)SerialportSetting.SerialportParams.Parity;
            serialPort.DataBits = SerialportSetting.SerialportParams.DataBits;
            serialPort.StopBits = (StopBits)(SerialportSetting.SerialportParams.StopBits + 1);
            serialPort.RtsEnable = SerialportSetting.SerialportParams.RtsEnable;
            serialPort.DtrEnable = SerialportSetting.SerialportParams.DtrEnable;

            NLogger.Info($"[UartOpen]open");
            serialPort.Open();
            lastPortBaseStream = serialPort.BaseStream;
            NLogger.Info($"[UartOpen]done");
        }

        /// <summary> 关闭串口 </summary>
        public void Close()
        {
            NLogger.Info($"[UartClose]refreshSerialDevice");
            refreshSerialDevice();
            NLogger.Info($"[UartClose]Close");
            serialPort.Close();
            NLogger.Info($"[UartClose]done");
        }

        /// <summary> 发送数据 </summary>
        /// <param name="data"> 数据内容 </param>
        public void SendData(byte[] data)
        {
            if (data.Length == 0)
                return;
            serialPort.Write(data, 0, data.Length);
            //Tools.Global.setting.SentCount += data.Length;
            UartDataSent(data, EventArgs.Empty);//回调
        }

        //收到串口事件的信号量
        public EventWaitHandle WaitUartReceive = new AutoResetEvent(true);

        /// <summary> 接收到事件 </summary>
        /// <param name="sender"> </param>
        /// <param name="e">      SerialDataReceivedEventArgs </param>
        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            WaitUartReceive.Set();
        }

        /// <summary> 单独开个线程接收数据 </summary>
        private void ReadData()
        {
            WaitUartReceive.Reset();
            while (true)
            {
                WaitUartReceive.WaitOne();
                if (Tools.Global.isMainWindowsClosed)
                    return;
                if (SerialportSetting.Timeout > 0)
                    Thread.Sleep(SerialportSetting.Timeout);//等待时间
                List<byte> result = new List<byte>();
                while (true)//循环读
                {
                    if (serialPort == null || !serialPort.IsOpen)//串口被关了，不读了
                        break;
                    try
                    {
                        int length = serialPort.BytesToRead;
                        if (length == 0)//没数据，退出去
                            break;
                        byte[] rev = new byte[length];
                        serialPort.Read(rev, 0, length);//读数据
                        if (rev.Length == 0)
                            break;
                        result.AddRange(rev);//加到list末尾
                    }
                    catch { break; }//崩了？

                    if (result.Count > SerialportSetting.MaxLength)//长度超了
                        break;
                    //if (Tools.Global.setting.bitDelay && Tools.Global.setting.timeout > 0)//如果是设置了等待间隔时间
                    //{
                    //    Thread.Sleep(Tools.Global.setting.timeout);//等待时间
                    //}
                }
                //Tools.Global.setting.ReceivedCount += result.Count;
                if (result.Count > 0)
                    try
                    {
                        var r = result.ToArray();
                        UartDataRecived(r, EventArgs.Empty);//回调事件

                        //LuaApis.SendChannelsReceived("uart", r);
                    }
                    catch { }
            }
        }
    }
}