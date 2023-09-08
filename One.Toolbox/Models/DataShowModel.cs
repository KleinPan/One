using System.Windows.Media;

namespace One.Toolbox.Models
{
    ////整个父类统一下
    //internal class DataShow
    //{
    //    public DateTime time { get; set; } = DateTime.Now;
    //    public string data;
    //}

    ///// <summary> 显示到日志显示页面的类 </summary>
    //internal class DataShowPara : DataShow
    //{
    //    public bool send;
    //}

    ///// <summary> 更通用的日志数据 </summary>
    //internal class DataShowRaw : DataShow
    //{
    //    public string title;
    //    public SolidColorBrush color;
    //}

    public class DataShowCommon
    {
        public DateTime CurrentTime { get; set; } = DateTime.Now;

        /// <summary> 消息前缀 </summary>
        public string Title;

        /// <summary> 消息前缀 </summary>
        public string Prefix;

        public string Content;

        public bool? Send;
        public SolidColorBrush PrefixColor;
        public SolidColorBrush MessageColor;

        public DataShowCommon()
        {
            PrefixColor = Brushes.Black;
            MessageColor = Brushes.Black;
        }

        public string TimeToString()
        {
            return CurrentTime.ToString("[yyyy/MM/dd HH:mm:ss.fff]");
        }
    }
}