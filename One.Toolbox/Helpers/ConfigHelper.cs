using One.Toolbox.Models;

using System.IO;

namespace One.Toolbox.Helpers
{
    internal class ConfigHelper
    {
        public static ConfigHelper Instance { get; set; } = new ConfigHelper();

        public string AppPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public const string Config = "Setting.json";
        public AllConfigModel AllConfig { get; set; } = new AllConfigModel();

        public ConfigHelper()
        {
        }

        public void Save()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(AllConfig);

            File.WriteAllText(AppPath + Config, json);
        }

        public void Load()
        {
            try
            {
                var text = File.ReadAllText(AppPath + Config);

                AllConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<AllConfigModel>(text);
            }
            catch (Exception)
            {
                InitDefaultData();
            }
        }

        void InitDefaultData()
        {
            AllConfig = new AllConfigModel();

            AllConfig.SerialportSetting.QuickSendList.Add(new Model.ToSendData()
            {
                Id = 0,
                Commit = "发送",
                Hex = false,
                Text = "Hello?",
            });

            AllConfig.SerialportSetting.QuickSendList.Add(new Model.ToSendData()
            {
                Id = 1,
                Commit = "Hex发送",
                Hex = true,
                Text = "01 02 03 04",
            });
        }
    }
}