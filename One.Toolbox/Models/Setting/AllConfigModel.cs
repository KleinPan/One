using One.Toolbox.Models.Serialport;

namespace One.Toolbox.Models.Setting
{
    internal class AllConfigModel
    {
        public SerialportSettingModel SerialportSetting { get; set; } = new SerialportSettingModel();
        public SerialportParams SerialportParams { get; set; } = new SerialportParams();

        public SettingModel Setting { get; set; } = new SettingModel();
    }
}