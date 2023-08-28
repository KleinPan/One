using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace One.Core.Helper.HttpHelper
{
    /// <summary> 官方推荐的HttpClient，下载方法使用上边的 </summary>
    public class HttpDownloadHelper
    {
        private static readonly HttpClient HttpClient;
        public static CancellationTokenSource source;

        static HttpDownloadHelper()
        {
            var handler = new SocketsHttpHandler() { };

            HttpClient = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };

            source = new CancellationTokenSource();
        }

        public static async void DownloadLittleAsync(string downloadUrl, string filePath, string fileName)
        {
            var httpclient = HttpDownloadHelper.HttpClient;

            //url = url + "?";

            var response = httpclient.GetByteArrayAsync(downloadUrl).Result;

            if (File.Exists(filePath + "//" + fileName))
            {
                File.Delete(filePath + "//" + fileName);
            }
            System.IO.FileStream fs;

            fs = new System.IO.FileStream(filePath + "//" + fileName, System.IO.FileMode.CreateNew);
            fs.Write(response, 0, response.Length);
            fs.Close();
            return;
        }

        public static async void Download(string downloadUrl, string filePath, string fileName)
        {
            var httpclient = HttpDownloadHelper.HttpClient;

            Progress<float> progress = new System.Progress<float>(x => NotifyProgress(x));
            //progress.ProgressChanged += Progress_ProgressChanged;

            // Create a file stream to store the downloaded data. This really can be any type of writeable stream.
            string filePathTemp = filePath + "//" + fileName;

            if (File.Exists(filePathTemp))
            {
                File.Delete(filePathTemp);
            }
            using (var file = new FileStream(filePathTemp, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                // Use the custom extension method below to download the data. The passed progress-instance will receive the download status updates.
                await httpclient.DownloadAsync(downloadUrl, file, progress, source.Token);
            }
        }

        public delegate void ProgressReport(float progress);

        public static event ProgressReport NotifyProgress;

        public static void StopDownLoad()
        {
            source.Cancel();
        }

        private static void Progress_ProgressChanged(object sender, float e)
        {
            // Console.WriteLine(e.ToString()); System.Diagnostics.Debug.WriteLine(e);
            NotifyProgress?.Invoke(e);
        }
    }

    public static class HttpClientExtensions
    {
        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<float> progress = null, CancellationToken cancellationToken = default)
        {
            // Get the http headers first to examine the content length
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var contentLength = response.Content.Headers.ContentLength;

                using (var download = await response.Content.ReadAsStreamAsync())
                {
                    // Ignore progress reporting when no progress reporter was passed or when the content length is unknown
                    if (progress == null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination);
                        return;
                    }

                    // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                    var relativeProgress = new Progress<long>(totalBytes => progress.Report((float)totalBytes / contentLength.Value));
                    // Use extension method to report progress while downloading
                    await download.CopyToAsync(destination, 81920, relativeProgress, cancellationToken);
                    progress.Report(1);
                }
            }
        }
    }

    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress = null, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }
}