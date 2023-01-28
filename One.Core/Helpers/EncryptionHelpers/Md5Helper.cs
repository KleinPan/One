using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace One.Core.Helpers.EncryptionHelpers
{
    public class Md5Helper
    {
        /// <summary> md5加密 </summary>
        /// <param name="content"> 要加密的内容 </param>
        /// <param name="isUpper"> 是否大写，默认小写 </param>
        /// <param name="is16">    是否是16位，默认32位 </param>
        /// <returns> </returns>
        public static string GetStringMd5(string content, bool isUpper = false, bool is16 = false)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
                string md5Str = BitConverter.ToString(result);
                md5Str = md5Str.Replace("-", "");
                md5Str = isUpper ? md5Str : md5Str.ToLower();
                return is16 ? md5Str.Substring(8, 16) : md5Str;
            }
        }

        public static string GetFileMd5(string path)
        {
            try
            {
                string res = "";
                MD5 md5 = MD5.Create();
                using (FileStream fs = File.OpenRead(path))
                {
                    string s = string.Empty;
                    byte[] b = md5.ComputeHash(fs);
                    for (int i = 0; i < b.Length; i++)
                    {
                        s += b[i].ToString("x2");
                    }
                    res = s;
                }
                md5.Clear();

                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}