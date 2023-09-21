using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Helpers;
using One.Toolbox.Models.Serialport;
using One.Toolbox.ViewModels.Serialport;

using System.Diagnostics;
using System.Windows.Input;

namespace One.Toolbox.Views.Serialport
{
    /// <summary> SerialportPage.xaml 的交互逻辑 </summary>
    public partial class SerialportPage
    {
        public ViewModels.Serialport.SerialportViewModel ViewModel { get; }

        public SerialportPage()
        {
            DataContext = ViewModel = App.Current.Services.GetService<SerialportViewModel>();

            InitializeComponent();
        }

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            QuickSendViewModel data = ((System.Windows.Controls.Button)sender).DataContext as QuickSendViewModel;
            Tuple<bool, string> ret = Tools.InputDialog.OpenDialog(ResourceHelper.FindStringResource("QuickSendSetButton"),
                data.Commit, ResourceHelper.FindStringResource("QuickSendChangeButton"));
            if (ret.Item1)
            {
                ((System.Windows.Controls.Button)sender).Content = data.Commit = ret.Item2;
            }
        }

        private void LogButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", PathHelper.logPath);
        }
    }
}