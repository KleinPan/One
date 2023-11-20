using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;
using One.Toolbox.Views.Settings;

namespace One.Toolbox.Views
{
    /// <summary> StickWindow.xaml 的交互逻辑 </summary>
    public partial class StickWindow
    {
        public StickWindow()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<StickWindowVM>();
            NonClientAreaContent = new NonClientAreaContentForStick();
        }
    }
}