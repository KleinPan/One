using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO.Ports;

namespace One.Control.Controls.SerialPortPack
{
 public   class SerialPortVM
    {
        #region 串口参数
        public ObservableCollection<string> SerialPortNameList { get; set; } = new ObservableCollection<string>();
        public string SerialPortName { get; set; }

        public int Baudrate { get; set; } = 9600;
        public ParityEnum Parity { get; set; } = ParityEnum.NONE;
        public StopBitsEnum StopBit { get; set; } = StopBitsEnum.USART_StopBits_1;
        public DataBitsEnum DataBit { get; set; } = DataBitsEnum.USART_WordLength_8b;

        #endregion

        public bool IsOpen { get; set; }

        public SerialPort SerialPort1 { get; set; }

        public delegate void MessageEvent(string message);

        public event MessageEvent NotifyMessage;


        public SerialPortVM()
        {

        }

        public SerialPort InitSerialPort()
        {
            if (SerialPort1 == null)
            {
                SerialPort1 = new SerialPort();
            }

            string[] slist = System.IO.Ports.SerialPort.GetPortNames();
            SerialPortNameList.Clear();
            foreach (var item in slist)
            {
                SerialPortNameList.Add(item);
            }

            if (SerialPortNameList.Count > 0)
            {
                SerialPortName = SerialPortNameList[0];
            }
            else
            {
                System.Windows.MessageBox.Show("没有串口可用！");
                return null;
            }

            if (NotifyMessage == null)
            {
                SerialPort1.DataReceived += SerialPort1_DataReceived;
            }

            return SerialPort1;
        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            NotifyMessage?.Invoke(SerialPort1.ReadExisting());
        }

        public void Open()
        {
            SerialPort1.Open();
        }

        public void SendMessage(byte[] mes)
        {
            if (SerialPort1.IsOpen)
            {
                SerialPort1.Write(mes, 0, mes.Length);
            }
        }

        public void Close()
        {
            try
            {
                SerialPort1.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
