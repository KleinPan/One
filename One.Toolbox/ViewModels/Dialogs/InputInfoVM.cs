using One.Toolbox.ViewModels.Base;

using System.Text.RegularExpressions;

namespace One.Toolbox.ViewModels.Dialogs;

public partial class InputInfoVM : BaseVM
{
    /// <summary> 唯一标识符，获取时候传入 </summary>
    [ObservableProperty] private string key;

    /// <summary> 标题 </summary>
    [ObservableProperty] private string title;

    /// <summary> 描述 </summary>
    [ObservableProperty] private string descrption;

    [ObservableProperty] private int dataLength;

    /// <summary> 正则校验规则 </summary>
    [ObservableProperty] private Regex dataRule;

    /// <summary> 结果 </summary>
    [ObservableProperty] private string content;

    public InputInfoVM(string title, string key)
    {
        Title = title;
        Key = key;
    }
}