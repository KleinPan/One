using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary> 计算字符串的特征码 </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public static string GetStringMD5(string source)
        {
            //计算字符串的MD5
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = string.Empty;
            for (int i = 0; i < md5Data.Length; i++)
            {
                //返回一个新字符串，该字符串通过在此实例中的字符左侧填充指定的
                //Unicode 字符来达到指定的总长度，从而使这些字符右对齐。
                // string num=12; num.PadLeft(4, '0'); 结果为为 '0012' 看字符串长度是否满足4位,
                //不满足则在字符串左边以"0"补足
                //调用Convert.ToString(整型,进制数) 来转换为想要的进制数
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }

            //使用 PadLeft 和 PadRight 进行轻松地补位
            destString = destString.PadLeft(32, '0');
            return destString;
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

        #region Net5.0

        //https://www.cnblogs.com/broadm/p/17789951.html

        /// <summary> 实例版 </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public static string HexConvert_Instance(string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }

        /// <summary> 性能最强 </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public static string HexConvert_Static(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = MD5.HashData(inputBytes);
            return Convert.ToHexString(hashBytes);
        }

        #endregion Net5.0
    }
}