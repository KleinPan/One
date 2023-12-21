namespace One.Toolbox.ViewModels.Serialport;

public partial class SerialportSettingVM : ObservableObject
{
    [ObservableProperty]
    private List<int> databitList = new List<int>() { 5, 6, 7, 8 };

    [ObservableProperty]
    private List<double> stopBits = new List<double>() { 1, 2, 3 };

    [ObservableProperty]
    private bool hexShow;

    [ObservableProperty]
    private bool hexSend;

    [ObservableProperty]
    private bool withExtraEnter;

    [ObservableProperty]
    private bool shortTimeInfo;

    /// <summary> 替换不可见字符 </summary>
    public bool EnableSymbol { get; set; }

    public int Timeout { get; set; }

    public int MaxLength { get; set; }

    public int MaxPacksAutoClear { get; set; }

    public bool LagAutoClear { get; set; }

    public SerialportParams SerialportParams { get; set; } = new SerialportParams();

    public List<QuickSendVM> QuickSendList { get; set; } = new List<QuickSendVM>();

    public SerialportSettingVM()
    {
        //SerialportParams = new SerialportParams();
    }

    public SerialportSettingModel ToModel()
    {
        SerialportSettingModel serialportSettingModel = new SerialportSettingModel();
        serialportSettingModel.HexShow = HexShow;
        serialportSettingModel.HexSend = HexSend;
        serialportSettingModel.WithExtraEnter = WithExtraEnter;
        serialportSettingModel.EnableSymbol = EnableSymbol;
        serialportSettingModel.Timeout = Timeout;
        serialportSettingModel.MaxLength = MaxLength;
        serialportSettingModel.MaxPacksAutoClear = MaxPacksAutoClear;
        serialportSettingModel.LagAutoClear = LagAutoClear;
        serialportSettingModel.QuickSendList = QuickSendList;
        serialportSettingModel.ShortTimeInfo = ShortTimeInfo;

        return serialportSettingModel;
    }
}