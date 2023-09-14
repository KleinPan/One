using HandyControl.Data;

using One.Toolbox.ViewModels.Base;

namespace One.Toolbox.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _appVersion = String.Empty;

    [ObservableProperty]
    private SkinType skinType = SkinType.Default;

    [ObservableProperty]
    private string currentLanguage;

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
    }
}