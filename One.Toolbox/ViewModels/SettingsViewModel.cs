// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using HandyControl.Data;
using One.Toolbox.ViewModels.Base;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace One.Toolbox.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _appVersion = String.Empty;

    [ObservableProperty]
    private SkinType skinType = SkinType.Default;

    [ObservableProperty]
    private string currentLanguage = "zh-CN";

    [ObservableProperty]
    private bool autoUpdate = true;

    public SettingsViewModel()
    {
        InitializeViewModel();
    }

    public override void InitializeViewModel()
    {
        //Tools.Global.LoadSetting();

        AppVersion = $"v{GetAssemblyVersion()} .NET 7.0";

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

    partial void OnSkinTypeChanged(SkinType skin)
    {
        var skins0 = App.Current.Resources.MergedDictionaries[0];
        skins0.MergedDictionaries.Clear();
        skins0.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetSkin(skin));
        skins0.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skin));

        var skins1 = App.Current.Resources.MergedDictionaries[1];
        skins1.MergedDictionaries.Clear();
        skins1.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        });
        skins1.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/One.Toolbox;component/Resources/Themes/Theme.xaml")
        });

        App.Current.MainWindow?.OnApplyTemplate();
    }
}