using One.Toolbox.ViewModels.Base;
using One.Toolbox.ViewModels.Serialport;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace One.Component
{
    internal class SerialPortComponent : BaseVM
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

        private SerialportSettingVM SerialportSetting { get; set; }

        /// <summary> 初始化串口各个触发函数 </summary>
        public SerialPortComponent()
        {
            //声明接收到事件
            serialPort.DataReceived += Serial_DataReceived;

            new Thread(ReadData).Start();
        }

        /// <summary> 刷新串口对象 </summary>
        private void refreshSerialDevice()
        {
            WriteTraceLog($"[refreshSerialDevice]start");

            try
            {
                WriteTraceLog($"[refreshSerialDevice]lastPortBaseStream.Dispose");
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
                WriteTraceLog($"[refreshSerialDevice]BaseStream.Dispose");
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
            WriteTraceLog($"[refreshSerialDevice]Dispose");
            Task.Run(() =>//我服了
            {
                try
                {
                    serialPort.Dispose();
                }
                catch { }
            });
            WriteTraceLog($"[refreshSerialDevice]new");
            lock (useless)//存起来
                useless.Add(serialPort);
            serialPort = new SerialPort();
            //声明接收到事件
            serialPort.DataReceived += Serial_DataReceived;

            WriteTraceLog($"[refreshSerialDevice]done");
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
        public void Open(SerialportSettingVM serialportParams)
        {
            string temp = serialPort.PortName;
            WriteTraceLog($"[UartOpen]refreshSerialDevice");
            refreshSerialDevice();
            serialPort.PortName = temp;

            SerialportSetting = serialportParams;

            serialPort.BaudRate = SerialportSetting.SerialportParams.BaudRate;
            serialPort.Parity = (Parity)SerialportSetting.SerialportParams.Parity;
            serialPort.DataBits = SerialportSetting.SerialportParams.DataBits;
            serialPort.StopBits = (StopBits)(SerialportSetting.SerialportParams.StopBits + 1);
            serialPort.RtsEnable = SerialportSetting.SerialportParams.RtsEnable;
            serialPort.DtrEnable = SerialportSetting.SerialportParams.DtrEnable;

            WriteTraceLog($"[UartOpen]open");
            serialPort.Open();
            lastPortBaseStream = serialPort.BaseStream;
            WriteTraceLog($"[UartOpen]done");
        }

        /// <summary> 关闭串口 </summary>
        public void Close()
        {
            WriteTraceLog($"[UartClose]refreshSerialDevice");
            refreshSerialDevice();
            WriteTraceLog($"[UartClose]Close");
            serialPort.Close();
            WriteTraceLog($"[UartClose]done");
        }

        /// <summary> 发送数据 </summary>
        /// <param name="data"> 数据内容 </param>
        public void SendData(byte[] data)
        {
            if (data.Length == 0)
                return;
            serialPort.Write(data, 0, data.Length);

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
                }

                if (result.Count > 0)
                {
                    try
                    {
                        var r = result.ToArray();
                        UartDataRecived(r, EventArgs.Empty);//回调事件
                    }
                    catch
                    { }
                }
            }
        }
    }
}