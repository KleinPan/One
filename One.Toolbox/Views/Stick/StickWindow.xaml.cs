using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Stick;
using One.Toolbox.Views.Settings;

using System.Windows.Controls;

namespace One.Toolbox.Views.Stick;

/// <summary> StickWindow.xaml 的交互逻辑 </summary>
public partial class StickWindow
{
    public StickWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<StickWindowVM>();
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
}