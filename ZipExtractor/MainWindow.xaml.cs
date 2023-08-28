using Ionic.Zip;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace ZipExtractor
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private BackgroundWorker _backgroundWorker;
        private readonly StringBuilder _logBuilder = new StringBuilder();

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _logBuilder.AppendLine(DateTime.Now.ToString("F"));
            _logBuilder.AppendLine();
            _logBuilder.AppendLine("ZipExtractor started with following command line arguments.");

            string[] args = Environment.GetCommandLineArgs();
            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                _logBuilder.AppendLine($"[{index}] {arg}");
            }

            _logBuilder.AppendLine();

            if (args.Length >= 4)
            {
                string executablePath = args[3];

                // Extract all the files.
                _backgroundWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };

                _backgroundWorker.DoWork += (o, eventArgs) =>
                {
                    foreach (var process in Process.GetProcesses())
                    {
                        try
                        {
                            if (process.MainModule.FileName.Equals(executablePath))
                            {
                                _logBuilder.AppendLine("Waiting for application process to exit...");

                                _backgroundWorker.ReportProgress(0, "Waiting for application to exit...");
                                process.WaitForExit();
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                        }
                    }

                    _logBuilder.AppendLine("BackgroundWorker started successfully.");

                    var path = args[2];

                    try
                    {
                        ReadOptions options = new ReadOptions();
                        options.Encoding = Encoding.Default;//设置编码，解决解压文件时中文乱码
                        using (ZipFile zip = ZipFile.Read(args[1], options))
                        {
                            for (int i = 0; i < zip.Entries.Count; i++)
                            {
                                zip[i].Extract(path, ExtractExistingFileAction.OverwriteSilently);//解压文件，如果已存在就覆盖

                                string currentFile = string.Format("Extracting {0}", zip[i].FileName);
                                int progress = (i + 1) * 100 / zip.Entries.Count;
                                _backgroundWorker.ReportProgress(progress, currentFile);

                                _logBuilder.AppendLine($"{currentFile} [{progress}%]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    /*
                    // Open an existing zip file for reading.
                    ZipStorer zip = ZipStorer.Open(args[1], FileAccess.Read);

                    // Read the central directory collection.
                    List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

                    _logBuilder.AppendLine($"Found total of {dir.Count} files and folders inside the zip file.");

                    for (var index = 0; index < dir.Count; index++)
                    {
                        if (_backgroundWorker.CancellationPending)
                        {
                            eventArgs.Cancel = true;
                            zip.Close();
                            return;
                        }

                        ZipStorer.ZipFileEntry entry = dir[index];
                        zip.ExtractFile(entry, System.IO.Path.Combine(path, entry.FilenameInZip));
                        string currentFile = string.Format("Extracting {0}", entry.FilenameInZip);
                        int progress = (index + 1) * 100 / dir.Count;
                        _backgroundWorker.ReportProgress(progress, currentFile);

                        _logBuilder.AppendLine($"{currentFile} [{progress}%]");
                    }

                    zip.Close();

                    */
                };

                _backgroundWorker.ProgressChanged += (o, eventArgs) =>
                {
                    progressBar.Value = eventArgs.ProgressPercentage;
                    textBoxInformation.Text = eventArgs.UserState.ToString();
                    //textBoxInformation.SelectionStart = textBoxInformation.Text.Length;
                    //textBoxInformation.SelectionLength = 0;
                };

                _backgroundWorker.RunWorkerCompleted += (o, eventArgs) =>
                {
                    try
                    {
                        if (eventArgs.Error != null)
                        {
                            throw eventArgs.Error;
                        }

                        if (!eventArgs.Cancelled)
                        {
                            textBoxInformation.Text = @"Finished";
                            try
                            {
                                ProcessStartInfo processStartInfo = new ProcessStartInfo(executablePath);
                                if (args.Length > 4)
                                {
                                    processStartInfo.Arguments = args[4];
                                }

                                Process.Start(processStartInfo);

                                _logBuilder.AppendLine("Successfully launched the updated application.");
                            }
                            catch (Win32Exception exception)
                            {
                                if (exception.NativeErrorCode != 1223)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        _logBuilder.AppendLine();
                        _logBuilder.AppendLine(exception.ToString());

                        MessageBox.Show(exception.Message, exception.GetType().ToString(),
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        _logBuilder.AppendLine();
                        App.Current.Shutdown();
                    }
                };

                _backgroundWorker.RunWorkerAsync();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _backgroundWorker?.CancelAsync();

            _logBuilder.AppendLine();
            File.AppendAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZipExtractor.log"),
                _logBuilder.ToString());
        }
    }
}