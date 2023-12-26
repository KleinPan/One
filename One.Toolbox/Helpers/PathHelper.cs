using One.Toolbox.Views.Stick;

using System.IO;

namespace One.Toolbox.Helpers
{
    internal class PathHelper
    {
        /// <summary> 当前程序路径,不带 \ </summary>
        public static string exePath { get; set; }

        public static string ConfigPath { get; set; }

        public static string ConfigPathCommon { get; set; }

        /// <summary> 当前程序路径 </summary>
        private static string basePath { get; set; } = System.IO.Directory.GetCurrentDirectory();

        /// <summary> 资源文件路径带\ </summary>
        public static string resourcePath { get; set; }

        /// <summary> 临时文件夹带\ </summary>
        public static string tempPath { get; set; }

        /// <summary> Log路径带\ </summary>
        public static string logPath { get; set; }

        /// <summary> 文档路径带\ </summary>
        public static string docPath { get; set; }

        /// <summary> 数据路径带\ </summary>
        public static string dataPath { get; set; }

        public static string stickPath { get; set; }
        public static string notePath { get; set; }

        /// <summary> BingImage路径带\ </summary>
        public static string imagePath { get; set; }

        static PathHelper()
        {
            //exePath = System.IO.Directory.GetCurrentDirectory();

            exePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            exePath = exePath.Substring(0, exePath.Length - 1);

            //Config
            ConfigPath = exePath + @"\Configs\";
            Directory.CreateDirectory(ConfigPath);

            resourcePath = exePath + @"\Resources\";
            //Directory.CreateDirectory(resourcePath);

            tempPath = exePath + @"\Temp\";
            Directory.CreateDirectory(tempPath);

            logPath = exePath + @"\Logs\";
            Directory.CreateDirectory(logPath);

            docPath = exePath + @"\Docs\";
            Directory.CreateDirectory(docPath);

            dataPath = exePath + @"\Data\";
            Directory.CreateDirectory(dataPath);

            stickPath = dataPath + @"Stick\";
            Directory.CreateDirectory(stickPath);

            notePath = dataPath + @"Note\";
            Directory.CreateDirectory(notePath);

            imagePath = exePath + @"\BingImages\";
            Directory.CreateDirectory(imagePath);
        }
    }
}