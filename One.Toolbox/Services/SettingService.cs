using HandyControl.Data;

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

            ChangSkinType(AllConfig.Setting.SkinType);
            ChangeLanguage(AllConfig.Setting.CurrentLanguage);
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

            AllConfig.SerialportSetting.QuickSendList.Add(new QuickSendVM()
            {
                Id = 0,
                Commit = "发送",
                Hex = false,
                Text = "Hello?",
            });

            AllConfig.SerialportSetting.QuickSendList.Add(new QuickSendVM()
            {
                Id = 1,
                Commit = "Hex发送",
                Hex = true,
                Text = "01 02 03 04",
            });
        }

        #endregion Operation

        #region Skin

        public void ChangSkinType(SkinType skin)
        {
            try
            {
                var skins0 = App.Current.Resources.MergedDictionaries[2];//APP.xaml 里边第二行
                skins0.MergedDictionaries.Clear();
                skins0.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetSkin(skin));
                skins0.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skin));

                var skins1 = App.Current.Resources.MergedDictionaries[3];
                skins1.MergedDictionaries.Clear();
                skins1.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
                });
                skins1.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/One.Toolbox;component/Resources/Themes/Theme.xaml")
                });

                App.Current.MainWindow?.OnApplyTemplate();

                AllConfig.Setting.SkinType = skin;

                Save();
            }
            catch (Exception ex)
            {
                MessageShowHelper.ShowErrorMessage(ex.Message);
            }
        }

        #endregion Skin

        #region Language

        public void ChangeLanguage(LanguageEnum currentLanguage)
        {
            try
            {
                //var cmb = (System.Windows.Controls.ComboBox)sender;

                //var selItem = (ComboBoxItem)cmb.SelectedValue;
                //var CurrentLanguage = selItem.Content.ToString();

                System.Windows.Application.Current.Resources.MergedDictionaries[0] = new System.Windows.ResourceDictionary()
                {
                    Source = new Uri($"pack://application:,,,/Resources/Languages/{currentLanguage}.xaml")
                };

                AllConfig.Setting.CurrentLanguage = currentLanguage;

                Save();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Resources.MergedDictionaries[0] = new System.Windows.ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,,/Resources/Languages/zh-CN.xaml", UriKind.RelativeOrAbsolute)
                };

                MessageShowHelper.ShowErrorMessage(ex.Message);
            }
        }

        #endregion Language
    }
}