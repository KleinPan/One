using One.Toolbox.Helpers;
using One.Toolbox.Views.Settings;

using System.Runtime.InteropServices;
using System.Windows.Controls;

using System.Windows.Media;

namespace One.Toolbox.Views.Stick;

/// <summary> StickWindow.xaml 的交互逻辑 </summary>
public partial class StickWindow
{
    public StickWindow()
    {
        InitializeComponent();
        //DataContext = App.Current.Services.GetService<StickItemVM>();
        NonClientAreaContent = new NonClientAreaContentForStick();
    }

    private bool topMost = false;

    private void ButtonPin_Click(object sender, RoutedEventArgs e)
    {
        Button thisButton = sender as Button;

        var a = thisButton.GetValue(HandyControl.Controls.IconElement.GeometryProperty);

        topMost = !topMost;

        Topmost = topMost;

        if (topMost)
        {
            thisButton.SetValue(HandyControl.Controls.IconElement.GeometryProperty, ResourceHelper.Dic["Pin20Regular"]);
        }
        else
        {
            thisButton.SetValue(HandyControl.Controls.IconElement.GeometryProperty, ResourceHelper.Dic["PinOff20Regular"]);
        }
    }

    private const int GWL_STYLE = -16;
    private const int WS_MAXIMIZEBOX = 0x00010000;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    private HideWindowHelper _hideWindowHelper;

    private void rootWnd_Loaded(object sender, RoutedEventArgs e)
    {
        IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        int style = GetWindowLong(hWnd, GWL_STYLE);

        // 移除 WS_MAXIMIZEBOX 样式，禁用最大化按钮
        style &= ~WS_MAXIMIZEBOX;

        SetWindowLong(hWnd, GWL_STYLE, style);
        //使用以上代码，你可以禁用 Windows 系统中应用程序贴边触发全屏或重新布局的行为。

        _hideWindowHelper = HideWindowHelper
             .CreateFor(this)
             .AddHider<HideOnLeft>()
             .AddHider<HideOnRight>()
             .AddHider<HideOnTop>();

        _hideWindowHelper.Start();
    }
}