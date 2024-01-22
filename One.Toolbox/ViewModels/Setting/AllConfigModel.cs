using One.Toolbox.ViewModels.Serialport;

namespace One.Toolbox.ViewModels.Setting;

public class AllConfigModel
{
    public SerialportSettingModel SerialportSetting { get; set; } = new SerialportSettingModel();
    public SerialportParams SerialportParams { get; set; } = new SerialportParams();

    public CommonSettingModel Setting { get; set; } = new CommonSettingModel();
    public List<EditFileInfo> EditFileInfoList { get; set; } = new();

    public NetSpeedSettingModel NetSpeedSetting { get; set; } = new NetSpeedSettingModel();
}

public class EditFileInfo
{
    public string FileName { get; set; }

    public string FilePath { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime ModifyTime { get; set; }
}

public class NetSpeedSettingModel
{
    public string LastAdapterName { get; set; }
    public bool ShowSpeedWndDefault { get; set; }
}