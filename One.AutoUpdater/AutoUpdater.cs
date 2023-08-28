using One.AutoUpdater.Interfaces;
using One.AutoUpdater.Models;
using One.AutoUpdater.Utilities;
using One.Core.Helpers;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace One.AutoUpdater
{
    /// <summary> 通过设置一些静态字段并执行其Start方法，可以自动更新应用程序的主类。 </summary>
    public static class AutoUpdater
    {
        #region 静态变量

        /// <summary> 如果您将zip文件用作更新文件，则可以将此值设置为应用程序的安装路径。仅当您的安装目录与可执行路径不同时才需要这样做。 </summary>
        public static string InstallationPath;

        /// <summary> 如果您的应用程序不需要管理员权限来替换旧版本，请将其设置为false。 </summary>
        public static bool RunUpdateAsAdmin = true;

        /// <summary> 包含有关应用程序最新版本信息的xml文件的URL。 </summary>
        public static string AppCastURL;

        private static System.Timers.Timer _remindLaterTimer;

        internal static bool Running;

        internal static Uri BaseUri;

        /// <summary> Set Proxy server to use for all the web requests in AutoUpdater.NET. </summary>
        public static IWebProxy Proxy;

        ///<summary>
        ///     AutoUpdater.NET will report errors if this is true.
        /// </summary>
        public static bool ReportErrors = true;

        /// <summary>
        /// Login/password/domain for FTP-request
        /// <para> FTP请求的登录名/密码/域 </para>
        /// </summary>
        public static NetworkCredential FtpCredentials;

        /// <summary>
        /// Set the User-Agent string to be used for HTTP web requests.
        /// <para> 设置用于HTTP Web请求的User-Agent字符串。 </para>
        /// </summary>
        public static string HttpUserAgent;

        /// <summary> Set Basic Authentication credentials required to download the file.设置下载文件所需的基本身份验证凭据。 </summary>
        public static IAuthentication BasicAuthDownload;

        ///<summary>
        ///     Set this to true if you want to ignore previously assigned Remind Later and Skip settings. It will also hide Remind Later and Skip buttons.
        ///     如果要忽略先前分配的“以后提醒”和“跳过”设置，请将其设置为true。它还将隐藏“稍后提醒”和“跳过”按钮。
        /// </summary>
        public static bool Mandatory;

        /// <summary> Set it to folder path where you want to download the update file. If not provided then it defaults to Temp folder. </summary>
        public static string DownloadPath;

        internal static string GetUserAgent()
        {
            return string.IsNullOrEmpty(HttpUserAgent) ? $"AutoUpdate_Bran" : HttpUserAgent;
        }

        /// <summary> Set the Application Title shown in Update dialog. Although AutoUpdater.NET will get it automatically, you can set this property if you like to give custom Title. </summary>
        public static string AppTitle;

        /// <summary> Set this to an instance implementing the IPersistenceProvider interface for using a data storage method different from the default Windows Registry based one. </summary>
        public static IPersistenceProvider PersistenceProvider;

        /// <summary> Set Basic Authentication credentials required to download the XML file. </summary>
        public static IAuthentication BasicAuthXML;

        #region Event

        /// <summary> A delegate type for hooking up parsing logic. </summary>
        /// <param name="args"> An object containing the AppCast file received from server. </param>
        public delegate void ParseUpdateInfoHandler(ParseUpdateInfoEventArgs args);

        /// <summary> An event that clients can use to be notified whenever the AppCast file needs parsing. </summary>
        public static event ParseUpdateInfoHandler ParseUpdateInfoEvent;

        #endregion Event

        /// <summary> If this is true users can see the skip button. </summary>
        public static bool ShowSkipButton = true;

        /// <summary> If this is true users can see the Remind Later button. </summary>
        public static bool ShowRemindLaterButton = true;

        /// <summary> A delegate type for hooking up update notifications. </summary>
        /// <param name="args"> An object containing all the parameters received from AppCast XML file. If there will be an error while looking for the XML file then this object will be null. </param>
        public delegate void CheckForUpdateEventHandler(UpdateInfoEventArgs args);

        /// <summary> An event that clients can use to be notified whenever the update is checked. </summary>
        public static event CheckForUpdateEventHandler CheckForUpdateEvent;

        /// <summary> A delegate type to handle how to exit the application after update is downloaded. </summary>
        public delegate void ApplicationExitEventHandler();

        /// <summary>
        /// An event that developers can use to exit the application gracefully.
        /// <para> 正常退出应用程序的事件。如果此项注册了自定义事件，则不会执行默认流程。 </para>
        /// </summary>
        public static event ApplicationExitEventHandler ApplicationExitEvent;

        #endregion 静态变量

        public static void Start(string appCast, NetworkCredential ftpCredentials, Assembly myAssembly = null)
        {
            FtpCredentials = ftpCredentials;
            Start(appCast, myAssembly);
        }

        /// <summary> Start checking for new version of application and display a dialog to the user if update is available. </summary>
        /// <param name="updateURL">  URL of the xml file that contains information about latest version of the application. </param>
        /// <param name="myAssembly"> Assembly to use for version checking. </param>
        public static void Start(string updateURL, Assembly myAssembly = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol |= (SecurityProtocolType)192 |
                                                        (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            }
            catch (NotSupportedException)
            {
            }

            if (Mandatory && _remindLaterTimer != null)
            {
                _remindLaterTimer.Stop();
                _remindLaterTimer.Close();
                _remindLaterTimer = null;
            }

            if (!Running && _remindLaterTimer == null)
            {
                Running = true;

                AppCastURL = updateURL;

                // Application.EnableVisualStyles();

                Assembly assembly = myAssembly ?? Assembly.GetEntryAssembly();

                using (var backgroundWorker = new BackgroundWorker())
                {
                    backgroundWorker.DoWork += (sender, args) =>
                    {
                        Assembly mainAssembly = args.Argument as Assembly;

                        args.Result = CheckUpdate(mainAssembly);
                    };

                    backgroundWorker.RunWorkerCompleted += (sender, args) =>
                    {
                        if (args.Error != null)
                        {
                            ShowError(args.Error);
                        }
                        else
                        {
                            if (!args.Cancelled)
                            {
                                if (StartUpdate(args.Result))
                                {
                                    //Running = false;
                                    return;
                                }
                            }
                        }

                        Running = false;
                    };

                    backgroundWorker.RunWorkerAsync(assembly);
                }
            }
        }

        internal static MyWebClient GetWebClient(Uri uri, IAuthentication basicAuthentication)
        {
            MyWebClient webClient = new MyWebClient
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
            };

            if (Proxy != null)
            {
                webClient.Proxy = Proxy;
            }

            if (uri.Scheme.Equals(Uri.UriSchemeFtp))
            {
                webClient.Credentials = FtpCredentials;
            }
            else
            {
                if (basicAuthentication != null)
                {
                    webClient.Headers[HttpRequestHeader.Authorization] = basicAuthentication.ToString();
                }

                webClient.Headers[HttpRequestHeader.UserAgent] = HttpUserAgent;
            }

            return webClient;
        }

        private static object CheckUpdate(Assembly mainAssembly)
        {
            var companyAttribute =
                (AssemblyCompanyAttribute)AssemblyHelper.GetAttribute(mainAssembly, typeof(AssemblyCompanyAttribute));
            string appCompany = companyAttribute != null ? companyAttribute.Company : "";

            if (string.IsNullOrEmpty(AppTitle))
            {
                var titleAttribute =
                    (AssemblyTitleAttribute)AssemblyHelper.GetAttribute(mainAssembly, typeof(AssemblyTitleAttribute));
                AppTitle = titleAttribute != null ? titleAttribute.Title : mainAssembly.GetName().Name;
            }

            string registryLocation = !string.IsNullOrEmpty(appCompany)
                ? $@"Software\{appCompany}\{AppTitle}\AutoUpdater"
                : $@"Software\{AppTitle}\AutoUpdater";

            if (PersistenceProvider == null)
            {
                PersistenceProvider = new RegistryPersistenceProvider(registryLocation);
            }

            BaseUri = new Uri(AppCastURL);

            UpdateInfoEventArgs args;
            using (MyWebClient client = GetWebClient(BaseUri, BasicAuthXML))
            {
                string xml = client.DownloadString(BaseUri);

                if (ParseUpdateInfoEvent == null)
                {
                    //XmlSerializer xmlSerializer = new XmlSerializer(typeof(UpdateInfoEventArgs));
                    //XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml)) { XmlResolver = null };
                    //args = (UpdateInfoEventArgs)xmlSerializer.Deserialize(xmlTextReader);
                    ParseUpdateInfoEventArgs parseArgs = new ParseUpdateInfoEventArgs(xml);
                    AutoUpdaterOnParseUpdateInfoEvent(parseArgs);
                    args = parseArgs.UpdateInfo;
                }
                else
                {
                    ParseUpdateInfoEventArgs parseArgs = new ParseUpdateInfoEventArgs(xml);
                    ParseUpdateInfoEvent(parseArgs);
                    args = parseArgs.UpdateInfo;
                }
            }

            if (string.IsNullOrEmpty(args.CurrentVersion) || string.IsNullOrEmpty(args.DownloadURL))
            {
                throw new MissingFieldException();
            }

            args.InstalledVersion = mainAssembly.GetName().Version;
            args.IsUpdateAvailable = new Version(args.CurrentVersion) > mainAssembly.GetName().Version;

            if (!Mandatory)
            {
                if (string.IsNullOrEmpty(args.Mandatory.MinimumVersion) ||
                    args.InstalledVersion < new Version(args.Mandatory.MinimumVersion))
                {
                    Mandatory = args.Mandatory.Value;
                }
            }

            if (Mandatory)
            {
                ShowRemindLaterButton = false;
                ShowSkipButton = false;
            }
            else
            {
                // Read the persisted state from the persistence provider. This method makes the persistence handling independent from the storage method.
                var skippedVersion = PersistenceProvider.GetSkippedVersion();
                if (skippedVersion != null)
                {
                    var currentVersion = new Version(args.CurrentVersion);
                    if (currentVersion <= skippedVersion)
                        return null;

                    if (currentVersion > skippedVersion)
                    {
                        // Update the persisted state. Its no longer makes sense to have this flag set as we are working on a newer application version.
                        PersistenceProvider.SetSkippedVersion(null);
                    }
                }

                var remindLaterAt = PersistenceProvider.GetRemindLater();
                if (remindLaterAt != null)
                {
                    int compareResult = DateTime.Compare(DateTime.Now, remindLaterAt.Value);

                    if (compareResult < 0)
                    {
                        return remindLaterAt.Value;
                    }
                }
            }

            return args;
        }

        private static void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            var json = System.Text.Json.JsonSerializer.Deserialize<UpdateInfoEventArgs>(args.RemoteData);
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = json.CurrentVersion,
                ChangelogURL = json.ChangelogURL,
                DownloadURL = json.DownloadURL,
                Mandatory = new Mandatory
                {
                    Value = json.Mandatory.Value,
                    //UpdateMode = json.mandatory.mode,
                    MinimumVersion = json.Mandatory.MinimumVersion
                },
                CheckSum = new CheckSum
                {
                    Value = json.CheckSum.Value,
                    HashingAlgorithm = json.CheckSum.HashingAlgorithm
                }
            };
        }

        private static bool StartUpdate(object result)
        {
            if (result is DateTime time)
            {
                SetTimer(time);
            }
            else
            {
                if (result is UpdateInfoEventArgs args)
                {
                    if (CheckForUpdateEvent != null)
                    {
                        CheckForUpdateEvent(args);
                    }
                    else
                    {
                        if (args.IsUpdateAvailable)
                        {
                            if (Mandatory)
                            {
                                DownloadUpdate(args);
                                Exit();
                            }
                            else
                            {
                                if (DownloadUpdate(args))
                                {
                                    Exit();
                                }
                                else
                                {
                                    Console.WriteLine("取消更新！");

                                    return false;
                                }
                            }

                            return true;
                        }

                        if (ReportErrors)
                        {
                            MessageBox.Show("没有更新。请稍后再试。",
                                "更新不可用",
                                 MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }

            return false;
        }

        internal static void SetTimer(DateTime remindLater)
        {
            TimeSpan timeSpan = remindLater - DateTime.Now;

            var context = SynchronizationContext.Current;

            _remindLaterTimer = new System.Timers.Timer
            {
                Interval = (int)timeSpan.TotalMilliseconds,
                AutoReset = false
            };

            _remindLaterTimer.Elapsed += delegate
            {
                _remindLaterTimer = null;
                if (context != null)
                {
                    try
                    {
                        context.Send(state => Start(), null);
                    }
                    catch (InvalidAsynchronousStateException)
                    {
                        Start();
                    }
                }
                else
                {
                    Start();
                }
            };

            _remindLaterTimer.Start();
        }

        public static void Start(Assembly myAssembly = null)
        {
            Start(AppCastURL, myAssembly);
        }

        /// <summary> Opens the Download window that download the update and execute the installer when download completes. </summary>
        public static bool DownloadUpdate(UpdateInfoEventArgs args)
        {
            //using (var downloadDialog = new MainWindow(args))
            //{
            //    try
            //    {
            //        return downloadDialog.ShowDialog().Equals(DialogResult.OK);
            //    }
            //    catch (TargetInvocationException)
            //    {
            //    }
            //}

            var downloadDialog = new UpdateWindow(args);
            var temp = downloadDialog.ShowDialog();

            return (bool)temp;
        }

        private static void ShowError(Exception exception)
        {
            if (ReportErrors)
            {
                if (exception is WebException)
                {
                    MessageBox.Show(
                        "连接更新服务器有问题。请检查您的网络连接并稍后再试。",
                        "更新不可用", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(exception.ToString(),
                        exception.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary> Detects and exits all instances of running assembly, including current. </summary>
        private static void Exit()
        {
            if (ApplicationExitEvent != null)
            {
                ApplicationExitEvent();
            }
            else
            {
                var currentProcess = Process.GetCurrentProcess();
                foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    string processPath;
                    try
                    {
                        processPath = process.MainModule.FileName;
                    }
                    catch (Win32Exception)
                    {
                        // Current process should be same as processes created by other instances of the application so it should be able to access modules of other instances. This means this is not the process we are looking for so we can safely skip this.
                        continue;
                    }

                    //get all instances of assembly except current
                    if (process.Id != currentProcess.Id && currentProcess.MainModule.FileName == processPath)
                    {
                        if (process.CloseMainWindow())
                        {
                            process.WaitForExit((int)TimeSpan.FromSeconds(10)
                                .TotalMilliseconds); //give some time to process message
                        }

                        if (!process.HasExited)
                        {
                            process.Kill(); //TODO show UI message asking user to close program himself instead of silently killing it
                        }
                    }
                }

                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        System.Windows.Application.Current.Shutdown()));
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        /// <summary> Shows standard update dialog. </summary>
        public static void ShowUpdateForm(UpdateInfoEventArgs args)
        {
        }
    }
}