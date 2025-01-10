using CommunityToolkit.Mvvm.Messaging;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Messenger;
using One.Toolbox.ViewModels;
using One.Toolbox.ViewModels.DataProcess;
using One.Toolbox.ViewModels.NetSpeed;
using One.Toolbox.ViewModels.Stick;

using System.Globalization;
using System.Windows.Threading;

namespace One.Toolbox
{
    /// <summary>App.xaml 的交互逻辑</summary>
    public partial class App : Application
    {
        /// <summary>Gets the current <see cref="App"/> instance in use</summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        public App()
        {
            Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
            Services = ConfigureServices();
            InitializeComponent();
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            InitDataColelection();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new CloseMessage());
            //处理后台线程杀不掉问题
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            base.OnExit(e);
        }

        /// <summary>Configures the services for the application.</summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // App Host
            //Scoped 1指定将为每个作用域创建服务的新实例。 在 ASP.NET Core 应用中，会针对每个服务器请求创建一个作用域。
            //Singleton	0指定将创建该服务的单个实例。
            //Transient 2指定每次请求服务时，将创建该服务的新实例。

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

            services.AddSingleton<ViewModels.Setting.CloudSettingsVM>();

            services.AddSingleton<ViewModels.BingImage.BingImagePageVM>();
            services.AddSingleton<ViewModels.LotteryDraw.LotteryDrawPageVM>();
            services.AddSingleton<ViewModels.FileMonitor.FileMonitorPageVM>();

            //Services
            services.AddSingleton<Services.SettingService>();

            services.AddSingleton<StickPageVM>();
            services.AddSingleton<TestPageVM>();
            services.AddSingleton<NetSpeedPageVM>();

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

        #region Exception

        /// <summary>Occurs when an exception is thrown by an application but not handled.</summary>
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
            //Crashes.ShouldProcessErrorReport = (ErrorReport report) =>
            //{
            //    // Check the report in here and return true or false depending on the ErrorReport.
            //     return true;
            //};

            if (exception.GetType() == typeof(System.ComponentModel.Win32Exception))
            {
                //return;
            }
            Tools.MessageBox.Show($"internal error from system!\r\n{exception.ToString()}\r\nexit!");
        }

        #endregion Exception
    }
}