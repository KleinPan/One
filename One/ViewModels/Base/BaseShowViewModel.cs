using Avalonia.Media;

using One.Core.Helpers.DataProcessHelpers;
using One.Toolbox.Component;

namespace One.Toolbox.ViewModels.Base;

public class BaseShowViewModel : BaseVM
{
    //protected FlowDocumentComponent flowDocumentHelper { get; set; }

    /// <summary> 显示消息的方法 </summary>
    /// <param name="title">   只支持字符串 </param>
    /// <param name="data">    </param>
    /// <param name="send">    </param>
    /// <param name="hexMode"> </param>
    public virtual void ShowData(string title = "", byte[] data = null, bool send = false, bool hexMode = false)
    {
        string realData = "";

        if (data != null)
        {
            if (hexMode)
            {
                realData = StringHelper.BytesToHexString(data);
            }
            else
            {
                realData = System.Text.Encoding.UTF8.GetString(data);
            }
        }

        DataShowCommon dataShowCommon = new DataShowCommon()
        {
            Title = title,
            Send = send,
            Content = realData,
            MessageColor = send ? new SolidColorBrush(Colors.DarkRed) : new SolidColorBrush(Colors.DarkGreen),
            Prefix = (send ? " << " : " >> "),
        };

        if (data == null)
        {
            dataShowCommon.Prefix = null;
        }
        //flowDocumentHelper.DataShowAdd(dataShowCommon);

        WriteInfoLog(realData);
    }
}