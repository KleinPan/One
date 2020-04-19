using System.Net;

namespace One.Core.Helper.HttpHelper
{
    public class CusHttpServerHelper : HttpServerHelper
    {
        public CusHttpServerHelper(int id)
        {
            EqID = id;
        }

        public int EqID { get; set; }

        public override void Dispather(HttpListenerContext context)
        {
            //base.Dispather(context);

            RequestHelper = new CusRequestHelper(context.Request, EqID);
            ResponseHelper = new ResponseHelper(context.Response);

            RequestHelper.DispatchResourcesString(fs =>
            {
                ResponseHelper.WriteToClient(fs);// 对相应的请求做出回应
            });
        }
    }

    internal class CusRequestHelper : RequestHelper
    {
        /*
        private IRealtimeDataService realtimeDataService;

        private IRealtimeYcDataService realtimeYcDataService;
        private IRealtimeYxDataService realtimeYxDataService;
        */
        public int EqID { get; set; }

        public int C1No { get; set; }
        public int C2No { get; set; }

        public CusRequestHelper(HttpListenerRequest request, int id) : base(request)
        {
            EqID = id;
            GetServices();
        }

        private void GetServices()
        {
            /*
           realtimeDataService = Locator.Current.GetService<IRealtimeDataService>();

           realtimeYxDataService = Locator.Current.GetService<IRealtimeYxDataService>();
           realtimeYcDataService = Locator.Current.GetService<IRealtimeYcDataService>();

           Yxdata camera1 = new Yxdata();
           camera1.ParamType = 11;
           //C1No = camera1.No = realtimeYxDataService.RealtimeDatas.Count + 1;
           C1No = camera1.No = 101;

           camera1.Name = "摄像头1识别结果";

           Yxdata camera2 = new Yxdata();
           camera1.ParamType = 11;
           //C2No = camera1.No = realtimeYxDataService.RealtimeDatas.Count + 1;
           C2No = camera1.No = 102;

           camera1.Name = "摄像头2识别结果";

           realtimeYxDataService.AddOrUpdate(camera1);
           realtimeYxDataService.AddOrUpdate(camera2);
           */
        }

        public override string ProcessingData(string receiveString)
        {
            /*
               var data = System.Text.Json.JsonSerializer.Deserialize<DataMessage>(receiveString);

               if (data.command_type == "3")
               {
                   if (data.equipment_id == EqID.ToString())
                   {
                       var yxdata1 = realtimeYxDataService.RealtimeDatas.Items.Where(x => x.No == 31).FirstOrDefault();
                       var yxdata2 = realtimeYxDataService.RealtimeDatas.Items.Where(x => x.No == 32).FirstOrDefault();

            //if (data.ipc_info.Count==2)
            //{
            //    yxdata1.Value = data.ipc_info[0].res_code;
            //    yxdata2.Value = data.ipc_info[1].res_code;
            //}
            //else
            //{
            //    if (data.ipc_info[0].object_type=="1")
            //    {
            //        yxdata1.Value = data.ipc_info[0].res_code;
            //    }
            //    else
            //    {
            //        yxdata2.Value = data.ipc_info[0].res_code;

            //    }
            //}

            if (data.ipc_info[0].object_type == "1")
                    {
                        yxdata1.Value = data.ipc_info[0].res_code;
                    }
                    else
                    {
                        yxdata2.Value = data.ipc_info[0].res_code;
                    }

                    realtimeYxDataService.AddOrUpdate(yxdata1);
                    realtimeYxDataService.AddOrUpdate(yxdata2);

                    ResponseInfo responseInfo = new ResponseInfo();
                    responseInfo.api_code = "001";

                    var jsonResponse = System.Text.Json.JsonSerializer.Serialize(responseInfo);

                    return jsonResponse;
                }
                else
                {
                    ResponseInfo responseInfo = new ResponseInfo();
                    responseInfo.api_code = "001";
                    responseInfo.res_code = "404";
                    responseInfo.res_msg = "failure";

                    var jsonResponse = System.Text.Json.JsonSerializer.Serialize(responseInfo);

                    return jsonResponse;
                }
            }
            else
            {
                ResponseInfo responseInfo = new ResponseInfo();
                responseInfo.api_code = "001";
                responseInfo.res_code = "404";
                responseInfo.res_msg = "failure";

                var jsonResponse = System.Text.Json.JsonSerializer.Serialize(responseInfo);

                return jsonResponse;
            }
            */
            return null;
        }
    }
}