using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

using One.AutoUpdater.Models;
using One.AutoUpdater.Utilities;

namespace One.AutoUpdater
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class UpdateWindow : Window
    {
        private MyWebClient _webClient;

        /// <summary> 临时文件夹路径 </summary>
        private string _tempFile;

        private DateTime _startedAt;

        private string filePath = string.Empty; //下载文件路径

        private bool isDownLoading = false; //是否正在下载标志
        private UpdateInfoEventArgs updateInfoEventArgs;

        //public bool result = false;
        public UpdateWindow(UpdateInfoEventArgs args)
        {
            InitializeComponent();

            updateInfoEventArgs = args;
            filePath = args.DownloadURL;

            Loaded += UpdateView_Loaded;

            btn_Cancel.Click += Btn_Cancel_Click;
            btn_Update.Click += Btn_Update_Click;
            Closing += MainWindow_Closing;

            txbTip.Text = $@"有新版本{args.CurrentVersion}可用，当前使用版本为{ args.InstalledVersion}";
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (_webClient == null)
            {
            }
            else if (_webClient.IsBusy)
            {
                _webClient.CancelAsync();
            }
            else
            {
            }
            // DialogResult = false;
        }

        private async void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Update.IsEnabled = false;
            this.isDownLoading = true;
            //await downloadFile();

            DownLoadWithFTP();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void UpdateView_Loaded(object sender, RoutedEventArgs e)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filePath);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            request.Credentials = AutoUpdater.FtpCredentials;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            bytes_total = response.ContentLength; //这是一个int成员变量，用于以后存储
                                                  // Console.WriteLine（Fetch Complete，ContentLength { 0}，response.ContentLength）;
            response.Close();

            lbl_size.Text = BytesToString(bytes_total);
        }

        private void DownLoadWithFTP()
        {
            var uri = new Uri(filePath);

            _webClient = AutoUpdater.GetWebClient(uri, AutoUpdater.BasicAuthDownload);

            if (string.IsNullOrEmpty(AutoUpdater.DownloadPath))
            {
                _tempFile = Path.GetTempFileName();
            }
            else
            {
                _tempFile = Path.Combine(AutoUpdater.DownloadPath, $"{Guid.NewGuid().ToString()}.tmp");
                if (!Directory.Exists(AutoUpdater.DownloadPath))
                {
                    Directory.CreateDirectory(AutoUpdater.DownloadPath);
                }
            }

            _webClient.DownloadProgressChanged += OnDownloadProgressChanged;

            _webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;

            _webClient.DownloadFileAsync(uri, _tempFile);
        }

        private long bytes_total = -1;

        private void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
        {
            this.isDownLoading = false;
            this.DialogResult = true;

            if (asyncCompletedEventArgs.Cancelled)
            {
                return;
            }

            try
            {
                if (asyncCompletedEventArgs.Error != null)
                {
                    throw asyncCompletedEventArgs.Error;
                }

                if (updateInfoEventArgs.CheckSum != null)
                {
                    CompareChecksum(_tempFile, updateInfoEventArgs.CheckSum);
                }

                ContentDisposition contentDisposition = null;
                if (_webClient.ResponseHeaders["Content-Disposition"] != null)
                {
                    contentDisposition = new ContentDisposition(_webClient.ResponseHeaders["Content-Disposition"]);
                }

                var fileName = string.IsNullOrEmpty(contentDisposition?.FileName)
                    ? Path.GetFileName(_webClient.ResponseUri.LocalPath)
                    : contentDisposition.FileName;

                //待解压文件路径
                var tempPath =
                    Path.Combine(
                        string.IsNullOrEmpty(AutoUpdater.DownloadPath) ? Path.GetTempPath() : AutoUpdater.DownloadPath,
                        fileName);

                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }

                File.Move(_tempFile, tempPath);

                string installerArgs = null;
                if (!string.IsNullOrEmpty(updateInfoEventArgs.InstallerArgs))
                {
                    installerArgs = updateInfoEventArgs.InstallerArgs.Replace("%path%",
                        Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
                }

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = tempPath,
                    UseShellExecute = true,
                    Arguments = installerArgs
                };

                var extension = Path.GetExtension(tempPath);

                #region zip

                if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    //string installerPath = Path.Combine(Path.GetDirectoryName(tempPath), "ZipExtractor.exe");

                    //File.WriteAllBytes(installerPath, Properties.Resources.ZipExtractor);

                    string executablePath = Process.GetCurrentProcess().MainModule.FileName;
                    string extractionPath = Path.GetDirectoryName(executablePath);

                    string installerPath = Path.Combine(extractionPath, "ZipExtractor.exe");

                    if (!string.IsNullOrEmpty(AutoUpdater.InstallationPath) &&
                        Directory.Exists(AutoUpdater.InstallationPath))
                    {
                        extractionPath = AutoUpdater.InstallationPath;
                    }

                    StringBuilder arguments =
                        new StringBuilder($"\"{tempPath}\" \"{extractionPath}\" \"{executablePath}\"");
                    string[] args = Environment.GetCommandLineArgs();
                    for (int i = 1; i < args.Length; i++)
                    {
                        if (i.Equals(1))
                        {
                            arguments.Append(" \"");
                        }

                        arguments.Append(args[i]);
                        arguments.Append(i.Equals(args.Length - 1) ? "\"" : " ");
                    }

                    processStartInfo = new ProcessStartInfo
                    {
                        FileName = installerPath,
                        UseShellExecute = true,
                        Arguments = arguments.ToString()
                    };
                }

                #endregion zip

                /*

                if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    string executablePath = Process.GetCurrentProcess().MainModule.FileName;
                    string extractionPath = Path.GetDirectoryName(executablePath);

                    ZipHelper.Decompression(tempPath, extractionPath);

                    processStartInfo = new ProcessStartInfo
                    {
                        FileName = executablePath,
                        UseShellExecute = true,
                        //Arguments = arguments.ToString()
                    };
                }
                 */

                #region msi

                else if (extension.Equals(".msi", StringComparison.OrdinalIgnoreCase))
                {
                    processStartInfo = new ProcessStartInfo
                    {
                        FileName = "msiexec",
                        Arguments = $"/i \"{tempPath}\""
                    };
                    if (!string.IsNullOrEmpty(installerArgs))
                    {
                        processStartInfo.Arguments += " " + installerArgs;
                    }
                }

                #endregion msi

                if (AutoUpdater.RunUpdateAsAdmin)
                {
                    processStartInfo.Verb = "runas";
                }

                try
                {
                    //启动 net core3.1项目时，需要添加启动配置文件

                    Process.Start(processStartInfo);

                    //ProcessStartInfo startInfo = new ProcessStartInfo("edge.exe");
                    //startInfo.WindowStyle = ProcessWindowStyle.Minimized;

                    //Process.Start(startInfo);

                    //startInfo.Arguments = "www.northwindtraders.com";

                    //Process.Start(startInfo);
                }
                catch (Win32Exception exception)
                {
                    if (exception.NativeErrorCode == 1223)
                    {
                        _webClient = null;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                _webClient = null;
            }
            finally
            {
                Close();
            }
        }

        private static void CompareChecksum(string fileName, CheckSum checksum)
        {
            using (var hashAlgorithm = HashAlgorithm.Create(
                    string.IsNullOrEmpty(checksum.HashingAlgorithm) ? "MD5" : checksum.HashingAlgorithm))
            {
                using (var stream = File.OpenRead(fileName))
                {
                    if (hashAlgorithm != null)
                    {
                        var hash = hashAlgorithm.ComputeHash(stream);
                        var fileChecksum = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

                        if (fileChecksum == checksum.Value.ToLower()) return;

                        throw new Exception("校验和不同");
                    }

                    throw new Exception("不支持的哈希算法");
                }
            }
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (_startedAt == default(DateTime))
            {
                _startedAt = DateTime.Now;
            }
            else
            {
                var timeSpan = DateTime.Now - _startedAt;
                long totalSeconds = (long)timeSpan.TotalSeconds;
                if (totalSeconds > 0)
                {
                    var bytesPerSecond = e.BytesReceived / totalSeconds;
                    lbl_speed.Text = BytesToString(bytesPerSecond) + "/s";
                }
            }

            //lbl_size.Text = BytesToString(e.TotalBytesToReceive);
            lbl_currentSize.Text = $@"{BytesToString(e.BytesReceived)}";
            //prob.Value = e.ProgressPercentage;

            double temp = (double)(e.BytesReceived) / (bytes_total);

            prob.Value = Math.Round(temp * 100, 2);
        }

        private static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
        }
    }
}