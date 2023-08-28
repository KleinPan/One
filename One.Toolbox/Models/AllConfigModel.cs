using One.Toolbox.Models.Serialport;

namespace One.Toolbox.Models
{
    internal class AllConfigModel
    {
        public SerialportSettingModel SerialportSetting { get; set; } = new SerialportSettingModel();
        public SerialportParams SerialportParams { get; set; } = new SerialportParams();
    }
}