using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Helpers;
using One.Toolbox.ViewModels;

using System.Windows.Controls;

namespace One.Toolbox.Views.Settings
{
    /// <summary> SettingHeader.xaml 的交互逻辑 </summary>
    public partial class NonClientAreaContent
    {
        public NonClientAreaContent()
        {
            var vm = App.Current.Services.GetService<SettingsViewModel>();
            vm.OnNavigatedEnter();
            DataContext = vm;
            InitializeComponent();
        }

        private bool topMost = false;

        private void ButtonPin_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;

            var a = thisButton.GetValue(HandyControl.Controls.IconElement.GeometryProperty);

            topMost = !topMost;

            App.Current.MainWindow.Topmost = topMost;

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
}