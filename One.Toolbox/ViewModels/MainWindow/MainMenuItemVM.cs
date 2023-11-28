using One.Toolbox.ViewModels.Base;

using System.Windows.Controls;

namespace One.Toolbox.ViewModels.MainWindow;

public partial class MainMenuItemVM : BaseVM
{
    [ObservableProperty]
    private UserControl content;

    [ObservableProperty]
    private string header;

    //[ObservableProperty]
    //private Type? targetPageType;

    [ObservableProperty]
    private object icon;

    /// <summary> 若要将子元素停靠到另一个方向，必须将 属性设置为 LastChildFillfalse,并且还必须为最后一个子元素指定显式停靠方向。 </summary>
    [ObservableProperty]
    public Dock dock;

    public MainMenuItemVM()
    {
        //Content = App.GetService< Type.GetType(nameof(targetPageType)) >();
        Dock = Dock.Top;
    }

    public override string ToString()
    {
        return Header.ToString();
    }
}