
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

    private void LanguageComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        try
        {
            var cmb = (System.Windows.Controls.ComboBox)sender;

            var selItem = (ComboBoxItem)cmb.SelectedValue;
            var CurrentLanguage = selItem.Content.ToString();

            System.Windows.Application.Current.Resources.MergedDictionaries[0] = new System.Windows.ResourceDictionary()
            {
                Source = new Uri($"pack://application:,,,/Resources/Languages/{CurrentLanguage}.xaml")
            };
        }
        catch
        {
            System.Windows.Application.Current.Resources.MergedDictionaries[0] = new System.Windows.ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/Resources/Languages/zh-CN.xaml", UriKind.RelativeOrAbsolute)
            };
        }
    }
}