using HandyControl.Data;

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Enums;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels;

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

                var select = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), langName);
                if (select == settingsViewModel.CurrentLanguage)
                {
                    return;
                }
                settingsViewModel.CurrentLanguage = select;
                var service = App.Current.Services.GetService<Services.SettingService>();
                service.ChangeLanguage(select);
            }
        }

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button { Tag: SkinType skinType })
            {
                PopupConfig.IsOpen = false;
                if (skinType.Equals(settingsViewModel.SkinType))
                {
                    return;
                }
                settingsViewModel.SkinType = skinType;
                var service = App.Current.Services.GetService<Services.SettingService>();
                service.ChangSkinType(skinType);
            }
        }
    }
}