using HandyControl.Controls;
using HandyControl.Data;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using One.Core.Helpers;
using One.Toolbox.Enums;
using One.Toolbox.Helpers;
using One.Toolbox.Services;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.ViewModels.Dashboard;
using One.Toolbox.Views.Stick;

using RestSharp;

using System.Diagnostics;

namespace One.Toolbox.ViewModels.Setting;

public partial class SettingsVM : BaseVM
{
    [ObservableProperty]
    private string _appVersion = String.Empty;

    //[ObservableProperty]
    //private SkinType skinType = SkinType.Default;

    //[ObservableProperty]
    //private LanguageEnum currentLanguage = LanguageEnum.zh_CN;

    [ObservableProperty]
    private bool autoUpdate = true;

    [ObservableProperty]
    private bool showStickOnStart;

    private SettingService SettingService { get; set; }

    public SettingsVM()
    {
        SettingService = App.Current.Services.GetService<Services.SettingService>();
        LoadSetting();
    }

    public override void OnNavigatedLeave()
    {
        base.OnNavigatedLeave();

        SaveSetting();
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
    }

    public override void InitializeViewModel()
    {
        AppVersion = $"v{GetAssemblyVersion()} .NET 8.0";

        Task.Run(async () =>
        {
            var a = await GetLatestInfo();

            if (a.NeedUpdate)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    HandyControl.Controls.Growl.Ask(new GrowlInfo
                    {
                        Message = $"New version v{a.Version} released,update it!",
                        CancelStr = ResourceHelper.FindStringResource("Cancel"),
                        ActionBeforeClose = isConfirmed =>
                        {
                            if (isConfirmed)
                            {
                                ProcessStartInfo sInfo = new(new Uri(a.DownloadURL).AbsoluteUri)
                                {
                                    UseShellExecute = true
                                };
                                Process.Start(sInfo);
                            }
                            else
                            {
                                Growl.Info("那下次吧!😥");
                            }

                            return true;
                        },
                    });
                });
            }
        });

        base.InitializeViewModel();

        if (ShowStickOnStart)
        {
            ShowStick();
        }
    }

    private static async Task<GithubReleaseFilterInfo> GetLatestInfo()
    {
        //https://docs.github.com/en/rest/releases/releases?apiVersion=2022-11-28#get-the-latest-release

        var options = new RestClientOptions("https://api.github.com/repos/KleinPan/One/releases/latest")
        {
        };
        var client = new RestClient(options);
        client.AddDefaultHeader("Accept", "application/vnd.github+json");
        var request = new RestRequest("");

        // The cancellation token comes from the caller. You can still make a call without it.
        var timeline = await client.GetAsync(request);

        if (timeline.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var githubReleaseInfoM = JsonConvert.DeserializeObject<GithubReleaseInfoM>(timeline.Content);

            GithubReleaseFilterInfo githubReleaseFilterInfo = new GithubReleaseFilterInfo();

            //var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

            //var localVersion =new AssemblyHelper(Assembly.GetExecutingAssembly()).ProductVersion;
            var localVersion = AssemblyHelper.Instance.ProductVersion;

            Version gitVersion = Version.Parse(githubReleaseInfoM.tag_name.Replace("v", ""));

            if (gitVersion > localVersion)
            {
                githubReleaseFilterInfo.NeedUpdate = true;
                githubReleaseFilterInfo.Version = gitVersion.ToString();
                githubReleaseFilterInfo.DownloadURL = githubReleaseInfoM.assets[0].browser_download_url;
            }
            return githubReleaseFilterInfo;
        }
        else
        {
            return new GithubReleaseFilterInfo()
            {
                NeedUpdate = false,
            };
        }
    }

    private string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
    }

    private void LoadSetting()
    {
        ShowStickOnStart = SettingService.AllConfig.Setting.ShowStickOnStart;
        AutoUpdate = SettingService.AllConfig.Setting.SutoUpdate;
    }

    private void SaveSetting()
    {
        SettingService.AllConfig.Setting.ShowStickOnStart = ShowStickOnStart;
        SettingService.AllConfig.Setting.SutoUpdate = AutoUpdate;

        SettingService.Save();
    }

    private StickWindow stickWindow;

    private void ShowStick()
    {
        if (stickWindow == null)
        {
            stickWindow = new StickWindow();
            stickWindow.Show();
        }
        else
        {
            stickWindow.Show();
        }
    }
}

public class SettingModel
{
    public SkinType SkinType;

    public LanguageEnum CurrentLanguage;

    public bool SutoUpdate;
    public bool ShowStickOnStart;

    public SettingModel()
    {
        SkinType = SkinType.Default;
        CurrentLanguage = LanguageEnum.zh_CN;
    }
}