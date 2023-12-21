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

    [ObservableProperty]
    public Dock dock;

    public MainMenuItemVM()
    {
        Dock = Dock.Top;
    }

    public override string ToString()
    {
        return Header.ToString();
    }
}