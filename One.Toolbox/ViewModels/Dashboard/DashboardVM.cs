﻿using One.Base.Helpers;
using One.Base.Helpers.NetHelpers;
using One.Toolbox.ViewModels.Base;

using RestSharp;

using System.Globalization;

namespace One.Toolbox.ViewModels.Dashboard;

public partial class DashboardVM : BaseVM
{
    public DashboardVM()
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
            Author = "--" + a.from;
        });

        _ = Task.Run(GetCurrentUseIP);
    }

    private void Register()
    {
        //https://blog.cool2645.com/posts/csruanjianjiaxul/
        //https://m.xp.cn/b.php/92230.html
        var regTime = RegistryHelper.ReadSetting("Toolbox", "FirstRun", "");
        if (string.IsNullOrEmpty(regTime))
        {
            var first = DateTime.Now.ToString("u");
            RegistryHelper.WriteKey("Toolbox", "FirstRun", first);
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

        var res = HIDInterfaceHelper.GetConnectedDevices(DeviceType.USBDevice);
    }

    private static async Task<YiyanAPIM> GetEveryDayYiyan()
    {
        var options = new RestClientOptions("https://v1.hitokoto.cn/")
        {
        };
        var client = new RestClient(options);

        var request = new RestRequest("");

        // The cancellation token comes from the caller. You can still make a call without it.
        var timeline = await client.GetAsync<YiyanAPIM>(request);

        return timeline;
    }

    #region StatusBar

    [ObservableProperty]
    private string currentUseIP;

    private void GetCurrentUseIP()
    {
        CurrentUseIP = NetHelper.GetLocalIP();
    }

    #endregion StatusBar
}