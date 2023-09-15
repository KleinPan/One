using HandyControl.Data;

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Enums;
using One.Toolbox.Models.Setting;
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

 

    private void LoadSetting()
    {
        
    }

    private void SaveSetting()
    { 
    }
}