using Newtonsoft.Json;

using One.Toolbox.Enums;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Serialport;
using One.Toolbox.ViewModels.Setting;

using System.IO;

namespace One.Toolbox.Services
{
    public class SettingService
    {
        #region Config

        public static string LocalConfig = PathHelper.ConfigPath + "Setting.json";
        public static string CloudConfig = PathHelper.ConfigPath + "CloudSetting.json";
        public AllConfigModel AllConfig { get; set; } = new AllConfigModel();

        #endregion Config

        public SettingService()
        {
            LoadLocalDefaultSetting();
        }

        #region Operation

        public void Save()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(AllConfig, Formatting.Indented, jsonSerializerSettings);

            File.WriteAllText(LocalConfig, json);
        }

        public void LoadLocalDefaultSetting()
        {
            LoadTargetSetting(LocalConfig);
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

            AllConfig.SerialportSetting.QuickSendList.Add(new QuickSendModel()
            {
                Id = 0,
                Commit = "发送",
                Hex = false,
                Text = "Hello?",
            });

            AllConfig.SerialportSetting.QuickSendList.Add(new()
            {
                Id = 1,
                Commit = "Hex发送",
                Hex = true,
                Text = "01 02 03 04",
            });
        }

        #endregion Operation
    }
}