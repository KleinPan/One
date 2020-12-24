using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using One.Control.EnumConverters;

namespace One.Control.Controls.SerialPortPack
{
    public static class SerialPortData
    {
        public static List<int> BaudRateList { get; set; } = new List<int>() { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
        public static List<string> SerialPortName { get; set; } = new List<string>() { "COM0", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9" };
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DataBitsEnum
    {
        [Description("7")]
        USART_WordLength_7b = 7,

        [Description("8")]
        USART_WordLength_8b = 8,

        [Description("9")]
        USART_WordLength_9b = 9
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ParityEnum
    {
        [Description("无")]
        NONE = 0,

        [Description("偶")]
        EVEN = 2,

        [Description("奇")]
        ODD = 1
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum StopBitsEnum
    {
        [Description("1")]
        USART_StopBits_1 = 1,

        [Description("2")]
        USART_StopBits_2 = 2
    }
}
