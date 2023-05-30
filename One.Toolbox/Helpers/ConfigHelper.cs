using One.Toolbox.Models;

using System.IO;

namespace One.Toolbox.Helpers
{
    internal class ConfigHelper
    {
        public static ConfigHelper Instance { get; set; } = new ConfigHelper();

        public string AppPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public const string Config = "config.json";
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
            }
        }
    }
}