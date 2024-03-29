namespace One.Toolbox.ViewModels.Serialport;

public class SerialportParams
{
    public int BaudRate { get; set; } = 115200;
    public int Parity { get; set; } = 0;
    public int DataBits { get; set; } = 8;
    public int StopBits { get; set; } = 1;

    /// <summary> Request To Send 请求发送 </summary>
    public bool RtsEnable { get; set; }

    /// <summary> Data Terminal Ready 数据终端准备好 </summary>
    public bool DtrEnable { get; set; } = true;
}