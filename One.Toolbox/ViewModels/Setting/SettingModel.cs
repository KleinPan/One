using HandyControl.Data;

using One.Toolbox.Enums;

namespace One.Toolbox.ViewModels.Setting;

public class SettingModel
{
    public SkinType SkinType;

    public LanguageEnum CurrentLanguage;

    public bool SutoUpdate;
    public bool ShowStickOnStart;

    public SettingModel()
    {
        SkinType = SkinType.Default;
        CurrentLanguage = LanguageEnum.zh_CN;
    }
}