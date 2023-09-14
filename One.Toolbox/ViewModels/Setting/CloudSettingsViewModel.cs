// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using ICSharpCode.AvalonEdit.Editing;

using One.Toolbox.Enums;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;

using System.IO;
using System.Net;

using WebDav;

namespace One.Toolbox.ViewModels;

public partial class CloudSettingsViewModel : BaseViewModel
{
    public static IWebDavClient _client = new WebDavClient();

    #region UI

    [ObservableProperty]
    private WebDAVTypeEnum selectedWebDAVTypeEnum;

    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private bool isUploading;

    [ObservableProperty]
    private bool isDownloading;

    #endregion UI

    private const string targetFile = targetDir + "/Setting.json";
    private const string targetDir = "One.Toolbox/Setting";

    public CloudSettingsViewModel()
    {
        InitializeViewModel();
    }

    [RelayCommand]
    private async void Upload()
    {
        IsUploading = true;
        await UploadCloudSettingAsync();
        IsUploading = false;
    }

    [RelayCommand]
    private void Download()
    {
        IsDownloading = true;
        DownloadCloudSettingAsync();
        IsDownloading = false;
    }

    private WebDavClientParams InitWebDavParam()
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

        return clientParams;
    }

    public async Task DownloadCloudSettingAsync()
    {
        var param = InitWebDavParam();
        using (var client = new WebDavClient(param))
        {
            // create a setting directory
            var resMK = await client.Mkcol(targetDir);

            if (!resMK.IsSuccessful)
            {
                MessageShowHelper.ShowErrorMessage("Connect failed!");
                return;
            }
            var res = await client.Propfind(targetFile);
            if (res.IsSuccessful)
            {
                if (res.Resources.Count > 0)
                {
                    var rsp = await client.GetRawFile(targetFile);

                    using (var fileStream = File.Create(ConfigHelper.Instance.AppPath + ConfigHelper.LocalConfig))
                    {
                        rsp.Stream.CopyTo(fileStream);
                    }

                    ConfigHelper.Instance.LoadTargetSetting(ConfigHelper.Instance.AppPath + ConfigHelper.LocalConfig);
                }

                MessageShowHelper.ShowErrorMessage("Not find cloud setting!");
            }
            else
            {
                MessageShowHelper.ShowErrorMessage("Not find cloud setting!");
            }
        }
    }

    public async Task UploadCloudSettingAsync()
    {
        var param = InitWebDavParam();
        using (var client = new WebDavClient(param))
        {
            // create a setting directory
            var resMK = await client.Mkcol(targetDir);
            if (!resMK.IsSuccessful)
            {
                MessageShowHelper.ShowErrorMessage("Connect failed!");
                return;
            }
            await client.PutFile(targetFile, File.OpenRead(ConfigHelper.Instance.AppPath + ConfigHelper.LocalConfig)); // upload a resource
        }
    }
}