using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary> 资源文件路径 </summary>
        public static string resourcePath { get; set; }

        /// <summary> 临时文件夹 </summary>
        public static string tempPath { get; set; }

        /// <summary> Log路径 </summary>
        public static string logPath { get; set; }

        /// <summary> </summary>
        public static string docPath { get; set; }

        static PathHelper()
        {
            //exePath = System.IO.Directory.GetCurrentDirectory();

            exePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            exePath = exePath.Substring(0, exePath.Length - 1);

            //Config
            ConfigPath = exePath + @"\Configs\";
            Directory.CreateDirectory(ConfigPath);

            ConfigPathCommon = ConfigPath + @"Common\";
            Directory.CreateDirectory(ConfigPathCommon);

            resourcePath = exePath + @"\Resources\";
            //Directory.CreateDirectory(resourcePath);

            tempPath = exePath + @"\Temp\";
            Directory.CreateDirectory(tempPath);

            logPath = exePath + @"\Logs\";
            Directory.CreateDirectory(logPath);

            docPath = exePath + @"\Docs\";
            Directory.CreateDirectory(docPath);
        }
    }
}