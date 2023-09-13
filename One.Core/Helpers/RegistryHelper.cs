using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One.Core.Helpers
{
    /// <summary>
    /// 注册表帮助类
    /// </summary>
    public class RegistryHelper
    {
        /// <summary>
        /// 写入注册表
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Setting"></param>
        public static bool WriteKey(string Section, string Key, string Setting)   
        {
            
            RegistryKey key1 = Registry.CurrentUser.CreateSubKey($"Software\\One\\{Section}");

            if (key1 == null)
            {
                return false;
            }
            try
            {
                key1.SetValue(Key, Setting);
                return true;
            }
            catch (Exception exception1)
            {
                return false;
            }
            finally
            {
                key1.Close();
            }
        }

        /// <summary>
        /// 读取注册表
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static string ReadSetting(string Section, string Key, string Default)
        {
            if (Default == null)
            {
                Default = "-1";
            }
          
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey($"Software\\One\\{Section}");
            if (key1 != null)
            {
                object obj1 = key1.GetValue(Key, Default);
                key1.Close();
                if (obj1 != null)
                {
                    if (!(obj1 is string))
                    {
                        return "-1";
                    }
                    string obj2 = obj1.ToString();
                   
                    return obj2;
                }
                return "-1";
            }
            return Default;
        }
    }
}
