using One.Core.ExtensionMethods;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace One.Core.Helpers.HttpHelper
{
    /// <summary> 主要用于get post请求 </summary>
    public class HTTPClientHelper
    {
        private static readonly HttpClient HttpClient;

        static HTTPClientHelper()
        {
            //var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None, ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true };
            var handler = new SocketsHttpHandler() { AutomaticDecompression = DecompressionMethods.None };

            HttpClient = new HttpClient(handler);
        }

        /// <summary> get请求，可以对请求头进行多项设置 </summary>
        /// <param name="paramArray"> </param>
        /// <param name="url">        </param>
        /// <returns> </returns>
        public static string GetRequestAsync(Dictionary<string, string> paramArray, string url)
        {
            string result = "";

            var httpclient = HTTPClientHelper.HttpClient;

            url = url + "?" + BuildParam(paramArray);
            var response = httpclient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Stream myResponseStream = response.Content.ReadAsStreamAsync().Result;
                //获取流的内容
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                result = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }

            return result;
        }

        public static string GetRequest(Dictionary<string, string> paramArray, string url)
        {
            var httpclient = HTTPClientHelper.HttpClient;

            url = url + "?" + BuildParam(paramArray);
            var result = httpclient.GetStringAsync(url).Result;
            return result;
        }

        public static string HttpPostDictionaryRequestAsync(string Url, Dictionary<string, string> paramArray)
        {
            return HttpPostRequestAsync(Url, paramArray, ContentType: "application/x-www-form-urlencoded");
        }

        /// <summary> 123 </summary>
        /// <param name="Url">     </param>
        /// <param name="jsonstr"> 234 </param>
        /// <returns> </returns>
        public static object HttpPostJsonRequestAsync(string Url, string jsonstr)
        {
            try
            {
                var taskResult = HttpPostRequestAsync(Url, jsonstr, ContentType: "application/json");
                var message = taskResult.Result;

                if (message != null && message.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (message)
                    {
                        var returnResult = message.Content.ReadAsStringAsync().Result;
                        //System.Diagnostics.Debug.WriteLine(returnResult);
                        return returnResult;

                        // result = message.Content.ReadAsStringAsync().Result;
                        //var bytes = message.Content.ReadAsByteArrayAsync().Result;
                    }
                }
                else
                {
                    // return Task.FromResult<string>("任务返回失败!");
                    return null;
                }
            }
            catch (Exception exception)
            {
                return exception;
            }
        }

        /// <summary> 异步POST请求 </summary>
        /// <param name="Url">         234 </param>
        /// <param name="paramArray">  324 </param>
        /// <param name="ContentType">
        /// <para> POST请求的两种编码格式:  </para>
        /// "application/x-www-urlencoded"是浏览器默认的编码格式,用于键值对参数,参数之间用&amp;间隔；
        /// <para> "multipart/form-data"常用于文件等二进制，也可用于键值对参数，最后连接成一串字符传输(参考Java OK HTTP)。 </para>
        /// <para> 除了这两个编码格式，还有"application/json"也经常使用。 </para>
        /// </param>
        /// <returns> </returns>
        public static string HttpPostRequestAsync(string Url, Dictionary<string, string> paramArray, string ContentType = "application/x-www-form-urlencoded")
        {
            string result = "";

            var postData = BuildParam(paramArray);

            var data = Encoding.ASCII.GetBytes(postData);

            try
            {
                HttpClient.DefaultRequestHeaders.Add("User-Agent", @"SmartSwitchCabinet.Client");
                HttpClient.DefaultRequestHeaders.Add("Accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");

                HttpResponseMessage message = null;
                using (Stream dataStream = new MemoryStream(data ?? new byte[0]))
                {
                    using (HttpContent content = new StreamContent(dataStream))
                    {
                        content.Headers.Add("Content-Type", ContentType);
                        //content.Headers.Add("Test", ContentType);

                        System.Diagnostics.Debug.WriteLine(content);

                        var task = HttpClient.PostAsync(Url, content);
                        message = task.Result;
                    }
                }
                if (message != null && message.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (message)
                    {
                        result = message.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary> 异步POST请求 </summary> <param name="Url">234</param> <param name="paramArray">324</param> <param name="ContentType"><para>POST请求的两种编码格式:</para>"application/x-www-urlencoded"是浏览器默认的编码格式,用于键值对参数,参数之间用&（&amp;）用间隔；<para>"multipart/form-data"常用于文件等二进制，也可用于键值对参数，最后连接成一串字符传输(参考Java OK HTTP)。</para><para>除了这两个编码格式，还有"application/json"也经常使用。</para></param> <returns></returns>
        private static Task<HttpResponseMessage> HttpPostRequestAsync(string Url, string jsonStr, string ContentType = "application/x-www-form-urlencoded")//"application/x-www-form-urlencoded"
        {
            var postData = jsonStr;

            System.Diagnostics.Debug.WriteLine("发送内容:\n" + postData);

            var data = Encoding.ASCII.GetBytes(postData);

            try
            {
                HttpClient.DefaultRequestHeaders.Add("User-Agent", @"SmartSwitchCabinet.Client");
                HttpClient.DefaultRequestHeaders.Add("Accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");

                HttpResponseMessage message = null;
                using (Stream dataStream = new MemoryStream(data ?? new byte[0]))
                {
                    using (HttpContent content = new StreamContent(dataStream))
                    {
                        content.Headers.Add("Content-Type", ContentType);
                        content.Headers.Add("Test", ContentType);

                        var task = HttpClient.PostAsync(Url, content);
                        //var task2=  task.ContinueWith(GetTimelyReturnMessages);
                        //  return task2;

                        return task;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return Task.FromResult<string>("函数执行异常!");
                return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        private static void GetTimelyReturnMessages(Task<HttpResponseMessage> message)
        {
        }

        private static string BuildParam(Dictionary<string, string> paramArray, Encoding encode = null)
        {
            string url = "";

            if (encode == null) encode = Encoding.UTF8;

            if (paramArray != null && paramArray.Count > 0)
            {
                var parms = "";
                foreach (var item in paramArray)
                {
                    parms += string.Format("{0}={1}&", item.Key.Encode(encode), item.Value.Encode(encode));
                }
                if (parms != "")
                {
                    parms = parms.TrimEnd('&');
                }
                url += parms;
            }
            return url;
        }
    }
}