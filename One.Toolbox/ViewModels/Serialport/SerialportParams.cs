namespace One.Toolbox.ViewModels.Serialport;

public class SerialportSettingModel
{
    public bool HexShow { get; set; }
    public bool HexSend { get; set; }
    public bool WithExtraEnter { get; set; }

    /// <summary> 替换不可见字符 </summary>
    public bool EnableSymbol { get; set; }

    public int Timeout { get; set; } = 50;

    public int MaxLength { get; set; } = 10240;

    /// <summary> 自动清空UI，在超过此包数 </summary>
    public int MaxPacksAutoClear { get; set; } = 200;

    public bool LagAutoClear { get; set; } = true;

    public List<QuickSendViewModel> QuickSendList { get; set; } = new List<QuickSendViewModel>();

    public SerialportSettingViewModel ToVM()
    {
        SerialportSettingViewModel serialportSettingModel = new SerialportSettingViewModel();
        serialportSettingModel.HexShow = HexShow;
        serialportSettingModel.HexSend = HexSend;
        serialportSettingModel.WithExtraEnter = WithExtraEnter;
        serialportSettingModel.EnableSymbol = EnableSymbol;
        serialportSettingModel.Timeout = Timeout;
        serialportSettingModel.MaxLength = MaxLength;
        serialportSettingModel.MaxPacksAutoClear = MaxPacksAutoClear;
        serialportSettingModel.LagAutoClear = LagAutoClear;
        serialportSettingModel.QuickSendList = QuickSendList;

        return serialportSettingModel;
    }
}

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