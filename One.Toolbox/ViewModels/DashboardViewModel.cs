// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using RestSharp;

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

    public class YiyanAPI
    {
        public int id { get; set; }
        public string uuid { get; set; }

        /// <summary> 正文 utf-8 </summary>
        public string hitokoto { get; set; }

        public string type { get; set; }

        /// <summary> 出处 </summary>
        public string from { get; set; }

        public string from_who { get; set; }
        public string creator { get; set; }
        public int creator_uid { get; set; }
        public int reviewer { get; set; }
        public string commit_from { get; set; }
        public string created_at { get; set; }
        public int length { get; set; }
    }
}