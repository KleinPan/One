using One.Toolbox.Models.Serialport;

namespace One.Toolbox.ViewModels.Serialport
{
    public class SerialportSettingViewModel
    {
        public bool HexShow { get; set; }
        public bool HexSend { get; set; }
        public bool WithExtraEnter { get; set; }

        /// <summary> 替换不可见字符 </summary>
        public bool EnableSymbol { get; set; }

        public SerialportParams SerialportParams { get; set; }

        public SerialportSettingViewModel()
        {
            SerialportParams = new SerialportParams();
        }
    }
}