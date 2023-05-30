using One.Toolbox.Component;
using One.Toolbox.Tools;

using System.Windows.Media;

namespace One.Toolbox.Models
{
    internal class DataRaw
    {
        public string time;
        public string title;
        public string data = null;
        public string hex = null;
        public SolidColorBrush color;

        public DataRaw(DataShowRaw d)
        {
            time = d.time.ToString("[yyyy/MM/dd HH:mm:ss.fff]");
            title = d.title;
            if (d.data != null && d.data.Length > 0)
            {
                var len = d.data.Length;
                var warn = "";
                if (d.data.Length > Global.setting.MaxPackShow)
                {
                    warn = FlowDocumentComponent.packLengthWarn;
                    len = Global.setting.MaxPackShow;
                }

                //主要数据

                data = d.data;
                //data = Global.setting.showHexFormat switch
                //{
                //    2 => Global.Byte2Hex(d.data, " ", len) + warn,
                //    _ => Global.Byte2Readable(d.data, len) + warn,
                //};
                color = d.color;
                //小字hex
                //if (Global.setting.showHexFormat == 0)
                //    hex = "HEX:" + Global.Byte2Hex(d.data, " ", len) + warn;
            }
        }
    }
}