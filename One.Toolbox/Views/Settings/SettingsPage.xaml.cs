using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.Setting;

namespace One.Toolbox.Views.Settings;

/// <summary> Interaction logic for SettingsPage.xaml </summary>
public partial class SettingsPage
{
    public SettingsPage()
    {
        DataContext = App.Current.Services.GetService<SettingsVM>();

        InitializeComponent();
    }
}