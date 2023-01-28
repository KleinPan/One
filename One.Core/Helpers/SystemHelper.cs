using System;
using System.Management;
using System.Text;

namespace One.Core.Helpers
{
    public class Computer
    {
        /// <summary> windows api 类型 </summary>
        public enum WindowsAPIType
        {
            /// <summary> 内存 </summary>
            Win32_PhysicalMemory,

            /// <summary> cpu </summary>
            Win32_Processor,

            /// <summary> 硬盘 </summary>
            win32_DiskDrive,

            /// <summary> 电脑型号 </summary>
            Win32_ComputerSystemProduct,

            /// <summary> 显卡,分辨率 </summary>
            Win32_VideoController,

            /// <summary> 操作系统 </summary>
            Win32_OperatingSystem,
        }

        public enum WindowsAPIKeys
        {
            /// <summary> 名称 </summary>
            Name,

            /// <summary> 显卡芯片 </summary>
            VideoProcessor,

            /// <summary> 显存大小 </summary>
            AdapterRAM,

            /// <summary> 分辨率宽 </summary>
            CurrentHorizontalResolution,

            /// <summary> 分辨率高 </summary>
            CurrentVerticalResolution,

            /// <summary> 电脑型号 </summary>
            Version,

            /// <summary> 硬盘容量 </summary>
            Size,

            /// <summary> 内存容量 </summary>
            Capacity,

            /// <summary> cpu核心数 </summary>
            NumberOfCores,

            PixelsPerXLogicalInch,

            PixelsPerYLogicalInch,
        }

        private static Computer _instance;
        private static readonly object _lock = new object();

        private Computer()
        { }

        public static Computer CreateComputer()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Computer();
                    }
                }
            }
            return _instance;
        }

        /// <summary> 查找cpu的名称，主频, 核心数 </summary>
        /// <returns> </returns>
        public Tuple<string, string> GetCPU()
        {
            Tuple<string, string> result = null;
            try
            {
                string str = string.Empty;
                ManagementClass mcCPU = new ManagementClass(WindowsAPIType.Win32_Processor.ToString());
                ManagementObjectCollection mocCPU = mcCPU.GetInstances();
                foreach (ManagementObject m in mocCPU)
                {
                    string name = m[WindowsAPIKeys.Name.ToString()].ToString();
                    string[] parts = name.Split('@');
                    result = new Tuple<string, string>(parts[0].Split('-')[0] + "处理器", parts[1]);
                    break;
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary> 获取cpu核心数 </summary>
        /// <returns> </returns>
        public string GetCPU_Count()
        {
            string str = "查询失败";
            try
            {
                int coreCount = 0;
                foreach (var item in new System.Management.ManagementObjectSearcher("Select * from " +
 WindowsAPIType.Win32_Processor.ToString()).Get())
                {
                    coreCount += int.Parse(item[WindowsAPIKeys.NumberOfCores.ToString()].ToString());
                }
                if (coreCount == 2)
                {
                    return "双核";
                }
                str = coreCount.ToString() + "核";
            }
            catch
            {
            }
            return str;
        }

        /// <summary> 获取系统内存大小 </summary>
        /// <returns> 内存大小（单位M） </returns>
        public string GetPhisicalMemory()
        {
            //用于查询一些如系统信息的管理对象
            ManagementObjectSearcher searcher = new ManagementObjectSearcher();
            searcher.Query = new SelectQuery(WindowsAPIType.Win32_PhysicalMemory.ToString(), "", new string[] { WindowsAPIKeys.Capacity.ToString() });//设置查询条件
            ManagementObjectCollection collection = searcher.Get();   //获取内存容量
            ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();

            long capacity = 0;
            while (em.MoveNext())
            {
                ManagementBaseObject baseObj = em.Current;
                if (baseObj.Properties[WindowsAPIKeys.Capacity.ToString()].Value != null)
                {
                    try
                    {
                        capacity += long.Parse(baseObj.Properties[WindowsAPIKeys.Capacity.ToString()].Value.ToString());
                    }
                    catch
                    {
                        return "查询失败";
                    }
                }
            }
            return ToGB(capacity, 1024.0);
        }

        /// <summary> 获取硬盘容量 </summary>
        public string GetDiskSize()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                string hdId = string.Empty;
                ManagementClass hardDisk = new ManagementClass(WindowsAPIType.win32_DiskDrive.ToString());
                ManagementObjectCollection hardDiskC = hardDisk.GetInstances();
                foreach (ManagementObject m in hardDiskC)
                {
                    long capacity = Convert.ToInt64(m[WindowsAPIKeys.Size.ToString()].ToString());
                    sb.Append(ToGB(capacity, 1000.0) + "+");
                }
                result = sb.ToString().TrimEnd('+');
            }
            catch
            {
            }
            return result;
        }

        /// <summary> 电脑型号 </summary>
        public string GetVersion()
        {
            string str = "查询失败";
            try
            {
                string hdId = string.Empty;
                ManagementClass hardDisk = new ManagementClass(WindowsAPIType.Win32_ComputerSystemProduct.ToString());
                ManagementObjectCollection hardDiskC = hardDisk.GetInstances();
                foreach (ManagementObject m in hardDiskC)
                {
                    str = m[WindowsAPIKeys.Version.ToString()].ToString(); break;
                }
            }
            catch
            {
            }
            return str;
        }

        /// <summary> 获取分辨率 </summary>
        public string GetFenbianlv()
        {
            string result = "查询失败";
            try
            {
                string hdId = string.Empty;
                ManagementClass hardDisk = new ManagementClass(WindowsAPIType.Win32_VideoController.ToString());
                ManagementObjectCollection hardDiskC = hardDisk.GetInstances();
                foreach (ManagementObject m in hardDiskC)
                {
                    result = m[WindowsAPIKeys.CurrentHorizontalResolution.ToString()].ToString() + "*" + m[WindowsAPIKeys.CurrentVerticalResolution.ToString()].ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        /// <summary> 显卡 芯片,显存大小 </summary>
        public Tuple<string, string> GetVideoController()
        {
            Tuple<string, string> result = null;
            try
            {
                ManagementClass hardDisk = new ManagementClass(WindowsAPIType.Win32_VideoController.ToString());
                ManagementObjectCollection hardDiskC = hardDisk.GetInstances();
                foreach (ManagementObject m in hardDiskC)
                {
                    result = new Tuple<string, string>(m[WindowsAPIKeys.VideoProcessor.ToString()].ToString().Replace("Family", ""),
                        ToGB(Convert.ToInt64(m[WindowsAPIKeys.AdapterRAM.ToString()].ToString()), 1024.0));
                    break;
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary> 操作系统版本 </summary>
        public string GetOS_Version()
        {
            string str = "";
            try
            {
                string hdId = string.Empty;
                ManagementClass hardDisk = new ManagementClass(WindowsAPIType.Win32_OperatingSystem.ToString());
                ManagementObjectCollection hardDiskC = hardDisk.GetInstances();
                foreach (ManagementObject m in hardDiskC)
                {
                    str = m[WindowsAPIKeys.Name.ToString()].ToString().Split('|')[0].Replace("Microsoft", "");
                    break;
                }
            }
            catch
            {
            }
            return str;
        }

        /// <summary> 将字节转换为GB </summary>
        /// <param name="size"> 字节值 </param>
        /// <param name="mod">  除数，硬盘除以1000，内存除以1024 </param>
        /// <returns> </returns>
        public static string ToGB(double size, double mod)
        {
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }
    }
}