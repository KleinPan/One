// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

namespace One.Toolbox.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _appVersion = String.Empty;

    //[ObservableProperty]
    //private Wpf.Ui.Appearance.ApplicationTheme currentTheme = Wpf.Ui.Appearance.ApplicationTheme.Unknown;

    [ObservableProperty]
    private string currentLanguage = "zh-CN";

    [ObservableProperty]
    private bool autoUpdate = true;

    public override void InitializeViewModel()
    {
        //Tools.Global.LoadSetting();

        //CurrentTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();

        AppVersion = $"{GetAssemblyVersion()}";

        base.InitializeViewModel();
    }

    private string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
    }

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

    partial void OnCurrentLanguageChanged(string value)
    {
        string real = value.Split(':')[1];
        CurrentLanguage = value;
    }

    //partial void OnCurrentThemeChanged(ApplicationTheme value)
    //{
    //    Wpf.Ui.Appearance.ApplicationThemeManager.Apply(value);
    //}
}