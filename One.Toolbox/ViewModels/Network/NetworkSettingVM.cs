using One.Toolbox.ViewModels.Base;
using One.Toolbox.ViewModels.Serialport;

namespace One.Toolbox.ViewModels.Network;

public partial class NetworkSettingVM : ObservableObject
{
    [ObservableProperty]
    private SendAndReceiveSettingVM sendAndReceiveSettingVM;

    [ObservableProperty]
    private string lastPort;

    public List<QuickSendVM> QuickSendList { get; set; } = new List<QuickSendVM>();

    public NetworkSettingVM()
    {
        //SerialportParams = new SerialportParams();
    }

    public NetworkSettingModel ToModel()
    {
        NetworkSettingModel model = new NetworkSettingModel();
        model.SendAndReceiveSettingModel = SendAndReceiveSettingVM.ToModel();
        model.QuickSendList = QuickSendList.Select(x => x.ToM()).ToList();
        model.LastPort = LastPort;
        return model;
    }
}

public partial class NetworkSettingModel
{
    public string LastPort { get; set; }
    public SendAndReceiveSettingModel SendAndReceiveSettingModel { get; set; } = new SendAndReceiveSettingModel();

    public List<QuickSendModel> QuickSendList { get; set; } = new List<QuickSendModel>();

    public NetworkSettingModel()
    {
    }

    public NetworkSettingVM ToVM()
    {
        NetworkSettingVM vm = new NetworkSettingVM();

        vm.SendAndReceiveSettingVM = SendAndReceiveSettingModel.ToVM();
        vm.QuickSendList = QuickSendList.Select(x => x.ToVM()).ToList();
        vm.LastPort = LastPort;

        return vm;
    }
}