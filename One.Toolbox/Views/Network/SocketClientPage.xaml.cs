using System.Windows.Controls;

namespace One.Toolbox.Views.Network
{
    /// <summary> SocketClientPage.xaml 的交互逻辑 </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class SocketClientPage
    {
        public SocketClientPage()
        {
            InitializeComponent();
        }

        private bool initial = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}