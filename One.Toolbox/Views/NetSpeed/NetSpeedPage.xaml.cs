using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.NetSpeed;

using System.Windows.Controls;

namespace One.Toolbox.Views.NetSpeed
{
    public partial class NetSpeedPage : UserControl
    {
        public NetSpeedPage()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetService<NetSpeedPageVM>();
        }
    }
}