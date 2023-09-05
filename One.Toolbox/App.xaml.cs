using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Windows.Threading;

namespace One.Toolbox
{
    /// <summary> App.xaml 的交互逻辑 </summary>
    public partial class App : Application
    {
        /// <summary> Gets the current <see cref="App"/> instance in use </summary>
        public new static App Current => (App)Application.Current;

        /// <summary> Gets the <see cref="IServiceProvider"/> instance to resolve application services. </summary>
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
            this.InitializeComponent();

            //AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Current.DispatcherUnhandledException += DispatcherOnUnhandledException;

            InitDataColelection();
        }

        /// <summary> Configures the services for the application. </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // App Host

            // Views and ViewModels
            //services.AddTransient<Views.MainWindow>();
            services.AddTransient<ViewModels.MainWindowViewModel>();

            //services.AddTransient<Views.Pages.DashboardPage>();
            services.AddTransient<ViewModels.DashboardViewModel>();

            //services.AddTransient<Views.Pages.StringConvertPage>();
            services.AddTransient<ViewModels.StringConvertViewModel>();

            //services.AddTransient<Views.Settings.SettingsPage>();
            services.AddTransient<ViewModels.SettingsViewModel>();

            //services.AddTransient<Views.Serialport.SerialportPage>();
            services.AddTransient<ViewModels.Serialport.SerialportViewModel>();

            //services.AddTransient<Views.Pages.NetworklPage>();
            services.AddTransient<ViewModels.Network.NetworkViewModel>();

            return services.BuildServiceProvider();
        }

        private void InitDataColelection()
        {
            var countryCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            AppCenter.SetCountryCode(countryCode);

            AppCenter.Start("fc53b46a-1bc7-4f67-8382-2f96c799223f",
               typeof(Analytics), typeof(Crashes));
        }

        // App Host

        // Page resolver service
        //services.AddSingleton<IPageService, PageService>();

        // Theme manipulation
        //services.AddSingleton<IThemeService, ThemeService>();

        // TaskBar manipulation
        //services.AddSingleton<ITaskBarService, TaskBarService>();
        //services.AddSingleton<IContentDialogService, ContentDialogService>();
        // Service containing navigation, same as INavigationWindow... but without window
        //services.AddSingleton<INavigationService, NavigationService>();

        //services.AddSingleton<ISnackbarService, SnackbarService>();

        // Main window with navigation

        // Configuration

        #region Exception

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
            Crashes.ShouldProcessErrorReport = (ErrorReport report) =>
            {
                // Check the report in here and return true or false depending on the ErrorReport.
                return true;
            };

            if (exception.GetType() == typeof(System.ComponentModel.Win32Exception))
            {
                Tools.MessageBox.Show($"internal error from system!\r\n{exception.Message}\r\nexit!");
                return;
            }

            Tools.MessageBox.Show($"{exception.Message}\r\nexit!");
            return;
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

        #endregion Exception
    }
}