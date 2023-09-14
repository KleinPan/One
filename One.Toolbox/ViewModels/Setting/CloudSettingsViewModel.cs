// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using One.Toolbox.ViewModels.Base;

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

    [RelayCommand]
    private   void Test()
    {
        MakeCallsAsync();
    }
    public async Task MakeCallsAsync()
    {

        string addr = "https://dav.jianguoyun.com/dav";
        var clientParams = new WebDavClientParams
        {
            BaseAddress = new Uri(addr),
            Credentials = new NetworkCredential("964012840@qq.com", "a2tyevkb6a2fe552")
        };

        using (var client = new WebDavClient(clientParams))
        {
            await client.Propfind("1.txt");
        }
        
 
      

    }
}