﻿// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using ICSharpCode.AvalonEdit.Editing;

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Enums;
using One.Toolbox.Helpers;
using One.Toolbox.Services;
using One.Toolbox.ViewModels.Base;

using System.IO;
using System.Net;

using WebDav;

namespace One.Toolbox.ViewModels;

public partial class CloudSettingsViewModel : BaseViewModel
{
    //https://github.com/skazantsev/WebDavClient/tree/main
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
    private SettingService settingService;
    public CloudSettingsViewModel()
    {
        InitializeViewModel();
    }

    public override void InitializeViewModel()
    {
        base.InitializeViewModel();


        settingService = App.Current.Services.GetService<Services.SettingService>();
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
        try
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

                        using (var fileStream = File.Create(settingService.AppPath + SettingService.LocalConfig))
                        {
                            rsp.Stream.CopyTo(fileStream);
                        }

                        settingService.LoadTargetSetting(settingService.AppPath + SettingService.LocalConfig);
                    }

                    MessageShowHelper.ShowErrorMessage("Not find cloud setting!");
                }
                else
                {
                    MessageShowHelper.ShowErrorMessage("Not find cloud setting!");
                }
            }
        }
        catch (Exception ex)
        {

            MessageShowHelper.ShowErrorMessage(ex.Message);
        }
       
    }

    public async Task UploadCloudSettingAsync()
    {
        try
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
                await client.PutFile(targetFile, File.OpenRead(settingService.AppPath + SettingService.LocalConfig)); // upload a resource
            }
        }
        catch (Exception ex)
        {

            MessageShowHelper.ShowErrorMessage(ex.Message);
        }
      
    }
}