using One.Toolbox.Helpers;
using One.Toolbox.Model;

using System.Windows.Controls;
using System.Windows.Input;

using Wpf.Ui.Controls.Navigation;

namespace One.Toolbox.Views.Pages
{
    /// <summary> SerialportPage.xaml 的交互逻辑 </summary>
    public partial class SerialportPage : INavigableView<ViewModels.SerialportViewModel>
    {
        public ViewModels.SerialportViewModel ViewModel { get; }

        public SerialportPage(ViewModels.SerialportViewModel viewModel)
        {
            DataContext = ViewModel = viewModel;

            InitializeComponent();
        }

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToSendData data = ((Button)sender).DataContext as ToSendData;
            Tuple<bool, string> ret = Tools.InputDialog.OpenDialog(ResourceHelper.FindStringResource("QuickSendSetButton"),
                data.Commit, ResourceHelper.FindStringResource("QuickSendChangeButton"));
            if (ret.Item1)
            {
                ((Button)sender).Content = data.Commit = ret.Item2;
            }
        }
    }
}