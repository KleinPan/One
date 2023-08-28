// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

namespace One.Toolbox.ViewModels;

public partial class SettingsViewModel : BaseViewModel, INavigationAware
{
    [ObservableProperty]
    private string _appVersion = String.Empty;

    [ObservableProperty]
    private Wpf.Ui.Appearance.ApplicationTheme currentTheme = Wpf.Ui.Appearance.ApplicationTheme.Unknown;

    public override void InitializeViewModel()
    {
        //Tools.Global.LoadSetting();

        CurrentTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();
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
                if (CurrentTheme == Wpf.Ui.Appearance.ApplicationTheme.Light)
                    break;

                Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light);
                CurrentTheme = Wpf.Ui.Appearance.ApplicationTheme.Light;

                break;

            default:
                if (CurrentTheme == Wpf.Ui.Appearance.ApplicationTheme.Dark)
                    break;

                Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Dark);
                CurrentTheme = Wpf.Ui.Appearance.ApplicationTheme.Dark;

                break;
        }
    }

    #endregion Theme
}