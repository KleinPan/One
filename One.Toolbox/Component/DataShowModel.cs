using System.Windows.Media;

namespace One.Toolbox.Component;

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