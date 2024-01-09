using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Services;
using One.Toolbox.ViewModels;
using One.Toolbox.ViewModels.DataProcess;
using One.Toolbox.ViewModels.MainWindow;
using One.Toolbox.Views;

using System.Globalization;

namespace One.Toolbox;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public new static App? Current => Application.Current as App;

    /// <summary> Gets the <see cref="IServiceProvider"/> instance to resolve application services. </summary>
    public IServiceProvider Services { get; private set; }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation. Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowVM()
            };

            Services = ConfigureServices(desktop);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainWindowVM()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary> Configures the services for the application. </summary>
    private static IServiceProvider ConfigureServices(IClassicDesktopStyleApplicationLifetime desktop)
    {
        var services = new ServiceCollection();

        // App Host
        //Scoped 1指定将为每个作用域创建服务的新实例。 在 ASP.NET Core 应用中，会针对每个服务器请求创建一个作用域。
        //Singleton	0指定将创建该服务的单个实例。
        //Transient 2指定每次请求服务时，将创建该服务的新实例。
        services.AddSingleton<IFilesService>(x => new FilesService(desktop.MainWindow));

        // Views and ViewModels
        services.AddSingleton<ViewModels.MainWindow.MainWindowVM>();

        //services.AddTransient<Views.Pages.DashboardPage>();
        services.AddSingleton<ViewModels.Dashboard.DashboardVM>();

        //services.AddTransient<Views.Pages.StringConvertPage>();
        services.AddSingleton<StringConvertPageVM>();

        //services.AddTransient<Views.Settings.SettingsPage>();
        services.AddSingleton<ViewModels.Setting.SettingsPageVM>();

        //services.AddTransient<Views.Serialport.SerialportPage>();
        services.AddSingleton<ViewModels.Serialport.SerialportPageVM>();

        //services.AddTransient<Views.Pages.NetworklPage>();
        services.AddSingleton<ViewModels.Network.NetworkPageVM>();

        services.AddSingleton<ViewModels.NotePad.NotePadPageVM>();
        services.AddSingleton<ViewModels.Setting.CloudSettingsVM>();

        services.AddSingleton<ViewModels.BingImage.BingImageVM>();
        services.AddSingleton<ViewModels.LotteryDraw.LotteryDrawPageVM>();

        //Services
        services.AddSingleton<Services.SettingService>();

        services.AddSingleton<TestPageVM>();

        //多例
        //services.AddTransient<StickItemVM>();

        return services.BuildServiceProvider();
    }

    private void InitDataColelection()
    {
        var countryCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
        AppCenter.SetCountryCode(countryCode);

        AppCenter.Start("fc53b46a-1bc7-4f67-8382-2f96c799223f",
           typeof(Analytics), typeof(Crashes));
    }
}