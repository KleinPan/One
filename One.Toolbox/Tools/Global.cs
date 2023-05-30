using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using One.Core.Helpers;
using One.Toolbox.Model;

using RestSharp;

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace One.Toolbox.Tools
{
    internal class Global
    {
        public static event EventHandler ProgramClosedEvent;

        //api接口文档网址

        //主窗口是否被关闭？
        private static bool _isMainWindowsClosed = false;

        public static bool isMainWindowsClosed
        {
            get
            {
                return _isMainWindowsClosed;
            }
            set
            {
                _isMainWindowsClosed = value;
                if (value)
                {
                    //uart.WaitUartReceive.Set();
                    //Logger.CloseUartLog();
                    //Logger.CloseLuaLog();
                    if (File.Exists(ProfilePath + "lock"))
                        File.Delete(ProfilePath + "lock");
                    ProgramClosedEvent?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        //给全局使用的设置参数项
        public static Model.Settings setting;

        //软件文件名
        private static string _fileName = "";

        public static string FileName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_fileName))
                {
                    using (var processModule = Process.GetCurrentProcess().MainModule)
                    {
                        _fileName = System.IO.Path.GetFileName(processModule?.FileName);
                    }
                }
                return _fileName;
            }
        }

        //软件根目录
        private static string _appPath = null;

        /// <summary> 软件根目录（末尾带\） </summary>
        public static string AppPath
        {
            get
            {
                if (_appPath == null)
                {
                    using (var processModule = Process.GetCurrentProcess().MainModule)
                    {
                        _appPath = System.IO.Path.GetDirectoryName(processModule?.FileName);
                    }
                    if (!_appPath.EndsWith("\\"))
                        _appPath = _appPath + "\\";
                }
                return _appPath;
            }
        }

        //配置文件路径（普通exe时，会被替换为AppPath）
        public static string ProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\One.Toolbox\";

        /// <summary> 获取实际的ProfilePath路径（目前没啥用了） </summary>
        /// <returns> </returns>
        public static string GetTrueProfilePath()
        {
            return ProfilePath;
        }

        /// <summary> 是否为应用商店版本？ </summary>
        /// <returns> </returns>
        public static bool IsMSIX()
        {
            return AppPath.ToUpper().Contains(@"\PROGRAM FILES\WINDOWSAPPS\");
        }

        /// <summary> 是否上报bug？低版本.net框架的上报行为将被限制 </summary>
        public static bool ReportBug { get; set; } = true;

        /// <summary> 是否有新版本？ </summary>
        public static bool HasNewVersion { get; set; } = false;

        /// <summary> 更换软件标题栏文字 </summary>
        public static event EventHandler<string> ChangeTitleEvent;

        public static void ChangeTitle(string s) => ChangeTitleEvent?.Invoke(null, s);

        /// <summary> 刷新lua脚本列表 </summary>
        public static event EventHandler RefreshLuaScriptListEvent;

        public static void RefreshLuaScriptList() => RefreshLuaScriptListEvent?.Invoke(null, null);

        /// <summary> 加载配置文件 </summary>
        public static void LoadSetting()
        {
            if (IsMSIX())
            {
                if (Directory.Exists(ProfilePath))
                {
                    //已经开过一次了，那就继续用之前的路径
                }
                else
                {
                    //appdata路径不可靠，用文档路径替代
                    ProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\One.Toolbox\\";
                    if (!Directory.Exists(ProfilePath))
                        Directory.CreateDirectory(ProfilePath);
                }
            }
            else
            {
                ProfilePath = AppPath;//普通exe时，直接用软件路径
            }
            //配置文件
            if (File.Exists(ProfilePath + "settings.json"))
            {
                try
                {
                    //cost 309ms
                    setting = JsonConvert.DeserializeObject<Model.Settings>(File.ReadAllText(ProfilePath + "settings.json"));
                    setting.SentCount = 0;
                    setting.ReceivedCount = 0;
                    setting.DisableLog = false;
                }
                catch
                {
                    Tools.MessageBox.Show($"配置文件加载失败！\r\n" +
                        $"如果是配置文件损坏，可前往{ProfilePath}settings.json.bakup查找备份文件\r\n" +
                        $"并使用该文件替换{ProfilePath}settings.json文件恢复配置");
                    Environment.Exit(1);
                }
            }
            else
            {
                if (Directory.GetFiles(ProfilePath).Length > 10)
                {
                    var r = Tools.InputDialog.OpenDialog("检测到当前文件夹有其他文件\r\n" +
                        "建议新建一个文件夹给One.Toolbox，并将One.Toolbox.exe放入其中\r\n" +
                        "不然当前文件夹会显得很乱哦~\r\n" +
                        "是否想要继续运行呢？", null, "温馨提示");
                    if (!r.Item1)
                        Environment.Exit(1);
                }
                setting = new Model.Settings();
            }
        }

        /// <summary> 软件打开后，所有东西的初始化流程 </summary>
        public static void Initial()
        {
            //文件名不能改！
            if (FileName.ToUpper() != "ONE.TOOLBOX.EXE")
            {
                Tools.MessageBox.Show("啊呀呀，软件文件名被改了。。。\r\n" +
                    "为了保证软件功能的正常运行，请将exe名改回One.Toolbox.exe");
                Environment.Exit(1);
            }
            //C:\Users\chenx\AppData\Local\Temp\7zO05433053\user_script_run
            if (AppPath.ToUpper().Contains(@"\APPDATA\LOCAL\TEMP\") ||
                AppPath.ToUpper().Contains(@"\WINDOWS\TEMP\"))
            {
                Tools.MessageBox.Show("请勿在压缩包内直接打开本软件。");
                Environment.Exit(1);
            }

            if (IsMSIX())//商店软件的文件路径需要手动新建文件夹
            {
                if (!Directory.Exists(ProfilePath))
                {
                    Directory.CreateDirectory(ProfilePath);
                }
                //升级的时候不会自动升级核心脚本，所以先强制删掉再释放，确保是最新的
                if (Directory.Exists(ProfilePath + "core_script"))
                    Directory.Delete(ProfilePath + "core_script", true);
            }

            //检测多开
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(processName);
            //如果该数组长度大于1，说明多次运行
            if (processes.Length > 1 && File.Exists(ProfilePath + "lock"))
            {
                Tools.MessageBox.Show("不支持同文件夹多开！\r\n如需多开，请在多个文件夹分别存放One.Toolbox.exe后，分别运行。");
                Environment.Exit(1);
            }
            if (!Directory.Exists(ProfilePath))
            {
                Directory.CreateDirectory(ProfilePath);
            }
            File.Create(ProfilePath + "lock").Close();

            //加载配置文件改成单独拎出来了

            //备份一下文件好了（心理安慰）
            if (File.Exists(ProfilePath + "settings.json"))
            {
                if (File.Exists(ProfilePath + "settings.json.bakup"))
                    File.Delete(ProfilePath + "settings.json.bakup");
                File.Copy(ProfilePath + "settings.json", ProfilePath + "settings.json.bakup");
            }
        }

        public static Encoding GetEncoding() => Encoding.GetEncoding(0);

        private static byte[] b_del = Encoding.GetEncoding(65001).GetBytes("␡");

        private static byte[][] symbols = null;

        /// <summary> byte转string（可读） </summary>
        /// <param name="vBytes"> </param>
        /// <returns> </returns>
        public static string Byte2Readable(byte[] vBytes, int len = -1)
        {
            if (len == -1)
                len = vBytes.Length;
            if (vBytes == null)//fix
                return "";
            //没开这个功能/非utf8就别搞了
            if (!setting.EnableSymbol || setting.encoding != 65001)
                return Helpers.ByteHelper.ByteToString(vBytes, len);
            //初始化一下这个数组
            if (symbols == null)
            {
                symbols = new byte[32][];
                string[] tc = { "␀", "␁", "␂", "␃", "␄", "␅", "␆", "␇", "␈", "␉", "␊", "␋", "␌", "␍",
                    "␎", "␏", "␐", "␑", "␒", "␓", "␔", "␕", "␖", "␗", "␘", "␙", "␚", "␛", "␜", "␝", "␞", "␟" };
                for (int i = 0; i < 32; i++)
                    symbols[i] = Encoding.GetEncoding(65001).GetBytes(tc[i]);
            }
            var tb = new List<byte>();
            for (int i = 0; i < len; i++)
            {
                switch (vBytes[i])
                {
                    case 0x0d:
                        //遇到成对出现
                        if (i < len - 1 && vBytes[i + 1] == 0x0a)
                        {
                            tb.AddRange(symbols[0x0d]);
                            tb.AddRange(symbols[0x0a]);
                            tb.Add(0x0d);
                            tb.Add(0x0a);
                            i++;
                        }
                        else
                        {
                            tb.AddRange(symbols[0x0d]);
                            tb.Add(vBytes[i]);
                        }
                        break;

                    case 0x0a:
                    case 0x09://tab字符
                        tb.AddRange(symbols[vBytes[i]]);
                        tb.Add(vBytes[i]);
                        break;

                    default:
                        //普通的字符
                        if (vBytes[i] <= 0x1f)
                            tb.AddRange(symbols[vBytes[i]]);
                        else if (vBytes[i] == 0x7f)//del
                            tb.AddRange(b_del);
                        else
                            tb.Add(vBytes[i]);
                        break;
                }
            }
            return GetEncoding().GetString(tb.ToArray());
        }

        /// <summary> 更换语言文件 </summary>
        /// <param name="languagefileName"> </param>
        public static void LoadLanguageFile(string languagefileName)
        {
            try
            {
                System.Windows.Application.Current.Resources.MergedDictionaries[0] = new System.Windows.ResourceDictionary()
                {
                    Source = new Uri($"pack://application:,,,/Resources/Languages/{languagefileName}.xaml", UriKind.RelativeOrAbsolute)
                };
            }
            catch
            {
                System.Windows.Application.Current.Resources.MergedDictionaries[0] = new System.Windows.ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,,/Resources/Languages/en-US.xaml", UriKind.RelativeOrAbsolute)
                };
            }
        }

        private static string GitHubToken = null;
    }
}