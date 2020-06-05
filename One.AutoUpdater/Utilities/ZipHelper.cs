using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace One.AutoUpdater.Utilities
{
  public  class ZipHelper
    {

        /// <summary>
        /// 解压ZIP文件
        /// </summary>
        /// <param name="strZipPath">待解压的ZIP文件</param>
        /// <param name="strUnZipPath">解压的目录</param>
        /// <param name="overWrite">是否覆盖</param>
        /// <returns>成功：true/失败：false</returns>
        public static bool Decompression(string strZipPath, string strUnZipPath, bool overWrite=true)
        {
            try
            {
                ReadOptions options = new ReadOptions();
                options.Encoding = Encoding.Default;//设置编码，解决解压文件时中文乱码
                using (ZipFile zip = ZipFile.Read(strZipPath, options))
                {
                    foreach (ZipEntry entry in zip)
                    {
                        if (string.IsNullOrEmpty(strUnZipPath))
                        {
                            strUnZipPath = strZipPath.Split('.').First();
                        }
                        if (overWrite)
                        {
                            entry.Extract(strUnZipPath, ExtractExistingFileAction.OverwriteSilently);//解压文件，如果已存在就覆盖
                        }
                        else
                        {
                            entry.Extract(strUnZipPath, ExtractExistingFileAction.DoNotOverwrite);//解压文件，如果已存在不覆盖
                        }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
