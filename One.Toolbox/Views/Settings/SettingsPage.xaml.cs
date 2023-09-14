using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

using System.Windows.Controls;

namespace One.Toolbox.Views.Settings;

/// <summary> Interaction logic for SettingsPage.xaml </summary>
public partial class SettingsPage
{
    public SettingsPage()
    {
        DataContext = App.Current.Services.GetService<SettingsViewModel>();

        InitializeComponent();
    }
}