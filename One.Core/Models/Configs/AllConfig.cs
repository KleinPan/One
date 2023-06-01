using System.Collections.Generic;

namespace One.Core.Models.Configs
{
    public class AllConfig
    {
        public AllConfig()
        {
        }

        public ConfigFileInfo ConfigInfo { get; set; } = new ConfigFileInfo();

        public BasicConfigInfo BasicConfigInfo { get; set; } = new BasicConfigInfo();

        public List<CommonConfigM> CommonConfigList { get; set; } = new List<CommonConfigM>();
        public List<TestSecquenceConfigM> AllSupportTestSecquences { get; set; } = new List<TestSecquenceConfigM>();

        public List<TestSecquenceConfigM> TestSecquences { get; set; } = new List<TestSecquenceConfigM>();
    }

    public class ConfigFileInfo
    {
        public string GenerateTime { get; set; }
        public string ModifyTime { get; set; }
        public string ModifyUser { get; set; }
        public string Version { get; set; }
    }

    public class BasicConfigInfo
    {
        public string Basic { get; set; }
    }
}