// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using Newtonsoft.Json;

using One.Core.Helpers;
using One.Toolbox.Helpers;
using One.Toolbox.Models.Dashboard;

using RestSharp;

using Wpf.Ui;

namespace One.Toolbox.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    public DashboardViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

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
                MessageShowHelper.ShowInfoMessage($"New Version => {a.Version}!\r\nDownloadURL => {a.DownloadURL} ");
            }
        });
    }

    [ObservableProperty]
    private string text;

    [ObservableProperty]
    private string author;

    [RelayCommand]
    private async void Test()
    {
        //string aa = @"D:\Program Files\Oray\SunLogin\SunloginClient\SunloginClient.exe";
        //Process.Start(aa);
        //HardwareHelper.SearchPortByLocation(DeviceType.ComPort, "PCIROOT(0)#PCI(1400)#USBROOT(0)#USB(10)", x => x.Count > 2);
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