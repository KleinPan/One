using System.Windows.Media;

namespace One.Toolbox.Models
{
    internal class DataUart
    {
        public string time;
        public string title;
        public string data;
        public string hex = null;
        public SolidColorBrush color;
        public SolidColorBrush hexColor;

        public DataUart(string data, DateTime time, bool sent)
        {
            if (string.IsNullOrEmpty(data))
                return;
            //byte[] temp = data.ToArray();

            this.time = time.ToString("[yyyy/MM/dd HH:mm:ss.fff]");
            title = sent ? " ← " : " → ";
            color = sent ? Brushes.DarkRed : Brushes.DarkGreen;
            hexColor = sent ? Brushes.IndianRed : Brushes.ForestGreen;

            //var len = temp.Length;
            //var warn = "";
            //if (temp.Length > Global.setting.MaxPackShow)
            //{
            //    warn = FlowDocumentComponent.packLengthWarn;
            //    len = Global.setting.MaxPackShow;
            //}
            //主要数据
            this.data = data;
            //if (temp != null && temp.Length > 0)
            //{
            //    this.data = Global.setting.showHexFormat switch
            //    {
            //        2 => Global.Byte2Hex(temp, " ", len) + warn,
            //        _ => Global.Byte2Readable(temp, len) + warn,
            //    };
            //}
            //同时显示模式时，才显示小字hex
            //if (Global.setting.showHexFormat == 0)
            //    hex = "HEX:" + Global.Byte2Hex(temp, " ", len) + warn;
        }
    }
}