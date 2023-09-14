// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using HandyControl.Controls;
using HandyControl.Data;

using Newtonsoft.Json;

using One.Core.Helpers;
using One.Toolbox.Helpers;
using One.Toolbox.Models.Dashboard;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.ViewModels.Dialogs;

using RestSharp;

using System.Diagnostics;
using System.Globalization;

namespace One.Toolbox.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    public DashboardViewModel()
    {
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    void InitData()
    {
        Register();

        Task.Run(async () =>
        {
            var a = await GetEveryDayYiyan();

            Text = a.hitokoto;
            Author = a.from;
        });

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
                        CancelStr = ResourceHelper.FindStringResource("LuaCancel"),
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
    }

    private void Register()
    {
        var regTime = One.Core.Helpers.RegistryHelper.ReadSetting("Toolbox", "FirstRun", "");
        if (string.IsNullOrEmpty(regTime))
        {
            var first = DateTime.Now.ToString("u");
            One.Core.Helpers.RegistryHelper.WriteKey("Toolbox", "FirstRun", first);
        }
        else
        {
            DateTime firstInfo = DateTime.ParseExact(regTime, "u", CultureInfo.InvariantCulture);

            var sub = DateTime.Now - firstInfo;

            if (sub > TimeSpan.FromDays(7))
            {
                // MessageShowHelper.ShowErrorMessage("试用期到期！");
            }
        }
    }

    [ObservableProperty]
    private string text;

    [ObservableProperty]
    private string author;

    [RelayCommand]
    private async void Test()
    {
        //var s = AssemblyHelper.Instance.FileVersionInfo;
        //var ab = new AssemblyHelper(Assembly.GetExecutingAssembly());

        //List<InputInfoVM> inputInfoVMs = new List<InputInfoVM>()
        //{
        //    new InputInfoVM("aa","bb"),
        //     new InputInfoVM("cc","dd"),
        //     new InputInfoVM("dd","ee"),
        //};
        //var res = await DialogHelper.Instance.ShowInputDialog("test", inputInfoVMs);
    }

    private static async Task<YiyanAPI> GetEveryDayYiyan()
    {
        var options = new RestClientOptions("https://v1.hitokoto.cn/")
        {
        };
        var client = new RestClient(options);

        var request = new RestRequest("");

        // The cancellation token comes from the caller. You can still make a call without it.
        var timeline = await client.GetAsync<YiyanAPI>(request);

        return timeline;
    }

    private static async Task<GithubReleaseFilterInfo> GetLatestInfo()
    {
        var options = new RestClientOptions("https://api.github.com/repos/KleinPan/One/releases/latest") //https://docs.github.com/en/rest/releases/releases?apiVersion=2022-11-28#get-the-latest-release
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
}