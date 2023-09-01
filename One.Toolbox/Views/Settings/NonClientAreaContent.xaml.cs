using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

namespace One.Toolbox.Views.Settings
{
    /// <summary> SettingHeader.xaml 的交互逻辑 </summary>
    public partial class NonClientAreaContent
    {
        public NonClientAreaContent()
        {
            DataContext = App.Current.Services.GetService<SettingsViewModel>();
            InitializeComponent();
        }
    }
}