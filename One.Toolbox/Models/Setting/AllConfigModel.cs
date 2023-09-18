using One.Toolbox.Models.Serialport;

namespace One.Toolbox.Models.Setting
{
    public class AllConfigModel
    {
        public SerialportSettingModel SerialportSetting { get; set; } = new SerialportSettingModel();
        public SerialportParams SerialportParams { get; set; } = new SerialportParams();

        public SettingModel Setting { get; set; } = new SettingModel();
        public List<EditFileInfo> EditFileInfoList { get; set; } = new();
    }

    public class EditFileInfo
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}