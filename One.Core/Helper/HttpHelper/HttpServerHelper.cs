using System;
using System.IO;
using System.Net;
using One.Core.ExtensionMethods;

namespace One.Core.Helper.HttpHelper
{
    public class HttpServerHelper
    {
        private HttpListener httpListener = new HttpListener();

        public void Setup(int port = 8907)
        {
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定匿名身份验证。
            httpListener.Prefixes.Add(string.Format("http://*:{0}/", port));//如果发送到8080 端口没有被处理，则这里全部受理，+是全部接收
            httpListener.Start();//开启服务

            Receive();//异步接收请求
        }

        public void Dispose()
        {
            httpListener.Stop();
            httpListener.Close();
        }

        private void Receive()
        {
            httpListener.BeginGetContext(new AsyncCallback(EndReceive), null);
        }

        private void EndReceive(IAsyncResult ar)
        {
            var context = httpListener.EndGetContext(ar);
            Dispather(context);//解析请求
            Receive();
        }

        public RequestHelper RequestHelper { get; set; }
        public ResponseHelper ResponseHelper { get; set; }

        public virtual void Dispather(HttpListenerContext context)
        {
            //HttpListenerRequest request = context.Request;
            //HttpListenerResponse response = context.Response;
            RequestHelper = new RequestHelper(context.Request);
            ResponseHelper = new ResponseHelper(context.Response);

            RequestHelper.DispatchResourcesString(fs =>
            {
                ResponseHelper.WriteToClient(fs);// 对相应的请求做出回应
            });
        }
    }

    public class RequestHelper
    {
        private HttpListenerRequest request;

        public RequestHelper(HttpListenerRequest request)
        {
            this.request = request;
        }

        public Stream RequestStream { get; set; }

        public void ExtracHeader()
        {
            RequestStream = request.InputStream;
        }

        public delegate void ExecutingDispatchFileStream(FileStream fs);

        public delegate void ExecutingDispatchString(byte[] byteArray);

        public void DispatchResourcesFileStream(ExecutingDispatchFileStream action)
        {
            //http://www.contoso.com/articles/recent.aspx中，原始 URL 为 /articles/recent.aspx。 原始 URL 包括查询字符串（如果存在）。
            var rawUrl = request.RawUrl;//资源默认放在执行程序的wwwroot文件下，默认文档为index.html
            string filePath = string.Format(@"{0}/wwwroot{1}", Environment.CurrentDirectory, rawUrl);//这里对应请求其他类型资源，如图片，文本等
            if (rawUrl.Length == 1)
                filePath = string.Format(@"{0}/wwwroot/index.html", Environment.CurrentDirectory);//默认访问文件
            try
            {
                if (File.Exists(filePath))
                {
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);

                    action?.Invoke(fs);
                }
            }
            catch { return; }
        }

        public void DispatchResourcesString(ExecutingDispatchString action)
        {
            if (request.HttpMethod == "POST")
            {
                try
                {
                    byte[] bytes = new byte[request.ContentLength64];
                    request.InputStream.Read(bytes, 0,(int) request.ContentLength64);

                    byte[] result = PreprocessingData(bytes);

                    action?.Invoke(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        public byte[] PreprocessingData(byte[] receiveBytes)
        {
            var receiveStr = receiveBytes.ToString(System.Text.Encoding.UTF8);
            //接收到的json
            System.Diagnostics.Debug.WriteLine("接收内容:\n" + receiveStr);

            //处理数据

            var res = ProcessingData(receiveStr);

            System.Diagnostics.Debug.WriteLine(res);

            var byteArray = res.Tobyte(System.Text.Encoding.UTF8);
            return byteArray;
        }

        public virtual string ProcessingData(string receiveString)
        {
            //var data = System.Text.Json.JsonSerializer.Deserialize<DataMessage>(receiveString);

            //ResponseInfo responseInfo = new ResponseInfo();

            //var jsonResponse = System.Text.Json.JsonSerializer.Serialize(responseInfo);

            return receiveString;
        }

        public void ResponseQuerys()
        {
            var querys = request.QueryString;
            foreach (string key in querys.AllKeys)
            {
                VarityQuerys(key, querys[key]);
            }
        }

        private void VarityQuerys(string key, string value)
        {
            switch (key)
            {
                case "pic": Pictures(value); break;
                case "text": Texts(value); break;
                default: Defaults(value); break;
            }
        }

        private void Pictures(string id)
        {
        }

        private void Texts(string id)
        {
        }

        private void Defaults(string id)
        {
        }
    }

    public class ResponseHelper
    {
        private HttpListenerResponse response;

        public ResponseHelper(HttpListenerResponse response)
        {
            this.response = response;
            OutputStream = response.OutputStream;
        }

        public Stream OutputStream { get; set; }

        public class FileObject
        {
            public FileStream fs;
            public byte[] buffer;
        }

        public void WriteToClient(byte[] byteArray)
        {
            response.StatusCode = 200;
            // byte[] buffer = new byte[byteArray.Length];
            //var temp = StringHelper.GetUTF8Byte(str);
            OutputStream.Write(byteArray, 0, byteArray.Length);
            OutputStream.Close();//关闭输出流，如果不关闭，浏览器将一直在等待状态
        }

        public void WriteToClient(FileStream fs)
        {
            response.StatusCode = 200;
            byte[] buffer = new byte[1024];
            FileObject obj = new FileObject() { fs = fs, buffer = buffer };
            fs.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(EndWrite), obj);
        }

        private void EndWrite(IAsyncResult ar)
        {
            var obj = ar.AsyncState as FileObject;
            var num = obj.fs.EndRead(ar);
            OutputStream.Write(obj.buffer, 0, num);
            if (num < 1)
            {
                obj.fs.Close(); //关闭文件流
                OutputStream.Close();//关闭输出流，如果不关闭，浏览器将一直在等待状态
                return;
            }
            obj.fs.BeginRead(obj.buffer, 0, obj.buffer.Length, new AsyncCallback(EndWrite), obj);
        }
    }
}
