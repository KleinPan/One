using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using One.Toolbox.Interfaces;
using One.Toolbox.Models;
using One.Toolbox.Services;

using System.IO;
using System.Reflection;
using System.Windows.Threading;

namespace One.Toolbox
{
    /// <summary> App.xaml 的交互逻辑 </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services. https://docs.microsoft.com/dotnet/core/extensions/generic-host https://docs.microsoft.com/dotnet/core/extensions/dependency-injection https://docs.microsoft.com/dotnet/core/extensions/configuration https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();
                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<ISnackbarService, SnackbarService>();

                // Main window with navigation
                services.AddScoped<IWindow, Views.MainWindow>();
                services.AddScoped<ViewModels.MainWindowViewModel>();

                // Views and ViewModels
                services.AddScoped<Views.Pages.MainContentPage>();
                services.AddScoped<ViewModels.MainContentViewModel>();

                services.AddScoped<Views.Pages.DashboardPage>();
                services.AddScoped<ViewModels.DashboardViewModel>();

                services.AddScoped<Views.Pages.StringConvertPage>();
                services.AddScoped<ViewModels.StringConvertViewModel>();

                services.AddScoped<Views.Pages.SettingsPage>();
                services.AddScoped<ViewModels.SettingsViewModel>();

                services.AddScoped<Views.Serialport.SerialportPage>();
                services.AddScoped<ViewModels.Serialport.SerialportViewModel>();

                services.AddScoped<Views.Pages.NetworklPage>();
                services.AddScoped<ViewModels.NetworkViewModel>();

                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
            }).Build();

        /// <summary> Gets registered service. </summary>
        /// <typeparam name="T"> Type of the service to get. </typeparam>
        /// <returns> Instance of the service or <see langword="null"/>. </returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary> Occurs when the application is loading. </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            //Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
            //var CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
#if DEBUG
#else
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Application.Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
#endif
        }

        /// <summary> Occurs when the application is closing. </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();

            Environment.Exit(0);
        }

        /// <summary> Occurs when an exception is thrown by an application but not handled. </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }

        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            SendReport(dispatcherUnhandledExceptionEventArgs.Exception);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            SendReport((Exception)unhandledExceptionEventArgs.ExceptionObject);
        }

        public static void SendReport(Exception exception, string developerMessage = "", bool silent = true)
        {
            if (exception.GetType() == typeof(System.ComponentModel.Win32Exception))
            {
                Tools.MessageBox.Show($"internal error from system!\r\n{exception.Message}\r\nexit!");
                return;
            }
            if (Tools.Global.setting.language == "zh-CN")
                Tools.MessageBox.Show("恭喜你触发了一个BUG！\r\n" +
                    "如果条件允许，请点击“Send Report”来上报这个BUG\r\n" +
                    $"报错信息：{exception.Message}");
            if (!Tools.Global.ReportBug)
            {
                Tools.MessageBox.Show("检测到不支持的.net版本，禁止上报bug");
                return;
            }
            if (Tools.Global.HasNewVersion)
            {
                Tools.MessageBox.Show("检测到该软件不是最新版，禁止上报bug\r\n请保证软件是最新版");
                return;
            }
        }
    }
}