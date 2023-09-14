using HandyControl.Data;

using One.Toolbox.Enums;
using One.Toolbox.Models.Setting;
using One.Toolbox.ViewModels.Base;

using System.Windows.Controls;

namespace One.Toolbox.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _appVersion = String.Empty;

    [ObservableProperty]
    private SkinType skinType = SkinType.Default;

    [ObservableProperty]
    private LanguageEnum currentLanguage = LanguageEnum.zh_CN;

    [ObservableProperty]
    private bool autoUpdate = true;

    private SettingModel settingModel = new SettingModel();

    public SettingsViewModel()
    {
    }

    public override void InitializeViewModel()
    {
        //Tools.Global.LoadSetting();

        AppVersion = $"v{GetAssemblyVersion()} .NET 7.0";
        LoadSetting();
        base.InitializeViewModel();
    }

    private string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
    }

    partial void OnSkinTypeChanged(SkinType skin)
    {
        var skins0 = App.Current.Resources.MergedDictionaries[1];//APP.xaml 里边第二行
        skins0.MergedDictionaries.Clear();
        skins0.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetSkin(skin));
        skins0.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skin));

        var skins1 = App.Current.Resources.MergedDictionaries[2];
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

        SaveSetting();
    }

    partial void OnCurrentLanguageChanged(LanguageEnum currentLanguage)
    {
        try
        {
            //var cmb = (System.Windows.Controls.ComboBox)sender;

            //var selItem = (ComboBoxItem)cmb.SelectedValue;
            //var CurrentLanguage = selItem.Content.ToString();

            System.Windows.Application.Current.Resources.MergedDictionaries[0] = new System.Windows.ResourceDictionary()
            {
                Source = new Uri($"pack://application:,,,/Resources/Languages/{currentLanguage}.xaml")
            };
        }
        catch
        {
            System.Windows.Application.Current.Resources.MergedDictionaries[0] = new System.Windows.ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/Resources/Languages/zh-CN.xaml", UriKind.RelativeOrAbsolute)
            };
        }

        SaveSetting();
    }

    private void LoadSetting()
    {
        Helpers.ConfigHelper.Instance.LoadLocalDefaultSetting();

        settingModel = Helpers.ConfigHelper.Instance.AllConfig.Setting;

        CurrentLanguage = settingModel.CurrentLanguage;
        SkinType = settingModel.SkinType;
    }

    private void SaveSetting()
    {
        try
        {
            SettingModel settingModel = new SettingModel();
            settingModel.SkinType = SkinType;

            settingModel.CurrentLanguage = CurrentLanguage;

            One.Toolbox.Helpers.ConfigHelper.Instance.AllConfig.Setting = settingModel;
            One.Toolbox.Helpers.ConfigHelper.Instance.Save();
        }
        catch (Exception)
        {
        }
    }
}