using HandyControl.Data;

using One.Toolbox.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One.Toolbox.Models.Setting
{
    public class SettingModel
    {
        public SkinType SkinType;

        public LanguageEnum CurrentLanguage;

        public SettingModel()
        {
            SkinType = SkinType.Default;
            CurrentLanguage = LanguageEnum.zh_CN;
        }
    }
}