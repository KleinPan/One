using HandyControl.Data;

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Enums;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Setting;

using System.Windows.Controls;

namespace One.Toolbox.Views.Settings
{
    /// <summary> SettingHeader.xaml 的交互逻辑 </summary>
    public partial class NonClientAreaContent
    {
        private SettingsViewModel settingsViewModel { get; set; }

        public NonClientAreaContent()
        {
            settingsViewModel = App.Current.Services.GetService<SettingsViewModel>();
            settingsViewModel.OnNavigatedEnter();
            DataContext = settingsViewModel;
            InitializeComponent();
        }

        private bool topMost = false;

        private void ButtonPin_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;

            var a = thisButton.GetValue(HandyControl.Controls.IconElement.GeometryProperty);

            topMost = !topMost;

            App.Current.MainWindow.Topmost = topMost;

            if (topMost)
            {
                thisButton.SetValue(HandyControl.Controls.IconElement.GeometryProperty, ResourceHelper.Dic["Pin20Regular"]);
            }
            else
            {
                thisButton.SetValue(HandyControl.Controls.IconElement.GeometryProperty, ResourceHelper.Dic["PinOff20Regular"]);
            }
        }

        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e)
        {
            PopupConfig.IsOpen = true;
        }

        private void ButtonLangs_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button { Tag: string langName })
            {
                PopupConfig.IsOpen = false;

                var service = App.Current.Services.GetService<Services.SettingService>();

                var select = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), langName);
                if (select == service.AllConfig.Setting.CurrentLanguage)
                {
                    return;
                }

                service.ChangeLanguage(select);
            }
        }

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button { Tag: SkinType skinType })
            {
                PopupConfig.IsOpen = false;

                var service = App.Current.Services.GetService<Services.SettingService>();

                if (skinType.Equals(service.AllConfig.Setting.SkinType))
                {
                    return;
                }

                service.ChangSkinType(skinType);
            }
        }
    }
}