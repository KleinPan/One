using One.Toolbox.ViewModels.Serialport;

namespace One.Toolbox.ViewModels.Base;

public partial class SendAndReceiveSettingVM : ObservableObject
{
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

    public List<QuickSendVM> QuickSendList { get; set; } = new List<QuickSendVM>();

    public SendAndReceiveSettingVM()
    {
        //SerialportParams = new SerialportParams();
    }

    public SendAndReceiveSettingModel ToModel()
    {
        SendAndReceiveSettingModel serialportSettingModel = new SendAndReceiveSettingModel();
        serialportSettingModel.HexShow = HexShow;
        serialportSettingModel.HexSend = HexSend;
        serialportSettingModel.WithExtraEnter = WithExtraEnter;
        serialportSettingModel.EnableSymbol = EnableSymbol;
        serialportSettingModel.Timeout = Timeout;
        serialportSettingModel.MaxLength = MaxLength;
        serialportSettingModel.MaxPacksAutoClear = MaxPacksAutoClear;
        serialportSettingModel.LagAutoClear = LagAutoClear;
        serialportSettingModel.QuickSendList = QuickSendList.Select(x => x.ToM()).ToList();
        serialportSettingModel.ShortTimeInfo = ShortTimeInfo;

        return serialportSettingModel;
    }
}

public class SendAndReceiveSettingModel
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

    public bool ShortTimeInfo { get; set; }

    public List<QuickSendModel> QuickSendList { get; set; } = new List<QuickSendModel>();

    public SendAndReceiveSettingVM ToVM()
    {
        SendAndReceiveSettingVM vm = new SendAndReceiveSettingVM();
        vm.HexShow = HexShow;
        vm.HexSend = HexSend;
        vm.WithExtraEnter = WithExtraEnter;
        vm.EnableSymbol = EnableSymbol;
        vm.Timeout = Timeout;
        vm.MaxLength = MaxLength;
        vm.MaxPacksAutoClear = MaxPacksAutoClear;
        vm.LagAutoClear = LagAutoClear;
        vm.QuickSendList = QuickSendList.Select(x => x.ToVM()).ToList();
        vm.ShortTimeInfo = ShortTimeInfo;

        return vm;
    }
}