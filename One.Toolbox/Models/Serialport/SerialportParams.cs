using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One.Toolbox.Models.Serialport
{
    public class SerialportParams
    {
        public int BaudRate { get; set; }
        public int Parity { get; set; }
        public int DataBits { get; set; }
        public int StopBits { get; set; }

        /// <summary> Request To Send 请求发送 </summary>
        public bool RtsEnable { get; set; }

        /// <summary> Data Terminal Ready 数据终端准备好 </summary>
        public bool DtrEnable { get; set; } = true;
    }
}