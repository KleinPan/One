// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;

using System.IO;
using System.Net;

using WebDav;

namespace One.Toolbox.ViewModels;

public partial class CloudSettingsViewModel : BaseViewModel
{
    public CloudSettingsViewModel()
    {
        InitializeViewModel();
    }

    public static IWebDavClient _client = new WebDavClient();

    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string password;

    [RelayCommand]
    private void Test()
    {
        GetCloudSettingAsync();
    }

    public async Task GetCloudSettingAsync()
    {
        string addr = "https://dav.jianguoyun.com/dav/";

        //var httpClient = new HttpClient();
        //httpClient.DefaultRequestHeaders.Authorization =
        //    new AuthenticationHeaderValue("964012840@qq.com", "a2tyevkb6a2fe552");

        var clientParams = new WebDavClientParams
        {
            BaseAddress = new Uri(addr),
            Credentials = new NetworkCredential(UserName, Password)
        };

        using (var client = new WebDavClient(clientParams))
        {
            // create a setting directory
            await client.Mkcol("One.Toolbox/Setting");

            var res = await client.Propfind("One.Toolbox/Setting/Setting.json");
            if (res.IsSuccessful)
            {
                if (res.Resources.Count > 0)
                {
                    var rsp = await client.GetRawFile("One.Toolbox/Setting/Setting.json");

                    using (var fileStream = File.Create(ConfigHelper.Instance.AppPath + ConfigHelper.CloudConfig))
                    {
                        rsp.Stream.CopyTo(fileStream);
                    }

                    ConfigHelper.Instance.LoadTargetSetting(ConfigHelper.Instance.AppPath + ConfigHelper.CloudConfig);
                }

                MessageShowHelper.ShowErrorMessage("Not find cloud setting!");
            }
            else
            {
                MessageShowHelper.ShowErrorMessage("Not find cloud setting!");
            }
        }
    }
}