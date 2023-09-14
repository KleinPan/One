using One.Toolbox.Models;
using One.Toolbox.Models.Serialport;

using System.IO;

namespace One.Toolbox.Helpers
{
    internal class ConfigHelper
    {
        public static ConfigHelper Instance { get; set; } = new ConfigHelper();

        public string AppPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public const string LocalConfig = "Setting.json";
        public const string CloudConfig = "CloudSetting.json";
        public AllConfigModel AllConfig { get; set; } = new AllConfigModel();

        public ConfigHelper()
        {
        }

        public void Save()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(AllConfig);

            File.WriteAllText(AppPath + LocalConfig, json);
        }

        public void LoadLocalDefaultSetting()
        {
            LoadTargetSetting(AppPath + LocalConfig);
        }

        public void LoadTargetSetting(string fullPath)
        {
            try
            {
                var text = File.ReadAllText(fullPath);

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

            AllConfig.SerialportSetting.QuickSendList.Add(new ToSendData()
            {
                Id = 0,
                Commit = "发送",
                Hex = false,
                Text = "Hello?",
            });

            AllConfig.SerialportSetting.QuickSendList.Add(new ToSendData()
            {
                Id = 1,
                Commit = "Hex发送",
                Hex = true,
                Text = "01 02 03 04",
            });
        }
    }
}