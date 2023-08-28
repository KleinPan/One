using One.Toolbox.Helpers;
using One.Toolbox.Model;

using System.Diagnostics;
using System.Windows.Input;

namespace One.Toolbox.Views.Serialport
{
    /// <summary> SerialportPage.xaml 的交互逻辑 </summary>
    public partial class SerialportPage : INavigableView<ViewModels.Serialport.SerialportViewModel>
    {
        public ViewModels.Serialport.SerialportViewModel ViewModel { get; }

        public SerialportPage(ViewModels.Serialport.SerialportViewModel viewModel)
        {
            DataContext = ViewModel = viewModel;

            InitializeComponent();
        }

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToSendData data = ((System.Windows.Controls.Button)sender).DataContext as ToSendData;
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