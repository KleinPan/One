using One.Toolbox.ViewModels.Base;

namespace One.Toolbox.ViewModels.Serialport;

public partial class SerialportSettingVM : ObservableObject
{
    [ObservableProperty]
    private List<int> databitList = new List<int>() { 5, 6, 7, 8 };

    [ObservableProperty]
    private List<double> stopBits = new List<double>() { 1, 2, 3 };

    [ObservableProperty]
    private SendAndReceiveSettingVM sendAndReceiveSettingVM;

    public SerialportParams SerialportParams { get; set; } = new SerialportParams();
    public List<QuickSendVM> QuickSendList { get; set; } = new List<QuickSendVM>();

    public SerialportSettingVM()
    {
    }

    public SerialportSettingModel ToModel()
    {
        SerialportSettingModel model = new SerialportSettingModel();
        model.SendAndReceiveSettingModel = SendAndReceiveSettingVM.ToModel();
        model.QuickSendList = QuickSendList.Select(x => x.ToM()).ToList();
        model.SerialportParams = SerialportParams;
        return model;
    }
}

public partial class SerialportSettingModel
{
    public SendAndReceiveSettingModel SendAndReceiveSettingModel { get; set; } = new SendAndReceiveSettingModel();
    public SerialportParams SerialportParams { get; set; } = new SerialportParams();
    public List<QuickSendModel> QuickSendList { get; set; } = new List<QuickSendModel>();

    public SerialportSettingModel()
    {
    }

    public SerialportSettingVM ToVM()
    {
        SerialportSettingVM vm = new SerialportSettingVM();
        vm.SerialportParams = SerialportParams;
        vm.SendAndReceiveSettingVM = SendAndReceiveSettingModel.ToVM();
        vm.QuickSendList = QuickSendList.Select(x => x.ToVM()).ToList();

        return vm;
    }
}