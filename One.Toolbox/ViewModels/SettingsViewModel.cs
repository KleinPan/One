// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using Wpf.Ui.Controls.Navigation;

namespace One.Toolbox.ViewModels;

public partial class SettingsViewModel : BaseViewModel, INavigationAware
{
    [ObservableProperty]
    private string _appVersion = String.Empty;

    [ObservableProperty]
    private Wpf.Ui.Appearance.ThemeType currentTheme = Wpf.Ui.Appearance.ThemeType.Light;

    private void InitializeViewModel()
    {
        Tools.Global.LoadSetting();

        CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
        AppVersion = $"{GetAssemblyVersion()}";

        base.InitializeViewModel();
    }

    private string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
    }

    [ObservableProperty]
    private string currentLanguage = "zh-CN";

    [RelayCommand]
    private void ChangeLanguage(string parameter)
    {
        //switch (parameter)
        //{
        //    case "zh-CN":

        // CurrentLanguage = "zh-CN"; break;

        // case "en-US":

        // CurrentLanguage = "en-US"; break;

        // default:

        //        break;
        //}

        CurrentLanguage = parameter;
        Tools.Global.setting.language = CurrentLanguage;
    }

    #region Theme

    [RelayCommand]
    private void OnChangeTheme(string parameter)
    {
        switch (parameter)
        {
            case "theme_light":
                if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Light)
                    break;

                Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
                CurrentTheme = Wpf.Ui.Appearance.ThemeType.Light;

                break;

            default:
                if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Dark)
                    break;

                Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
                CurrentTheme = Wpf.Ui.Appearance.ThemeType.Dark;

                break;
        }
    }

    #endregion Theme
}