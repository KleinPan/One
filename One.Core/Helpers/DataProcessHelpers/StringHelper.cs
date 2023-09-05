using Org.BouncyCastle.Utilities.Encoders;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace One.Core.Helpers.DataProcessHelpers
{
    public class StringHelper
    {
        #region String-Bytes-Base64

        /// <summary> 须为 <see cref="System.Text.Encoding.UTF8"/>&gt; </summary>
        /// <param name="plainText"> </param>
        /// <returns> </returns>
        public static string Base64Encode(byte[] plainText)
        {
            return System.Convert.ToBase64String(plainText);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Base64Encode(plainTextBytes);
        }

        public static byte[] Base64DecodeToByte(string base64EncodedData)
        {
            return System.Convert.FromBase64String(base64EncodedData);
        }

        public static string Base64DecodeToString(string base64EncodedData)
        {
            var base64EncodedBytes = Base64DecodeToByte(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion String-Bytes-Base64

        #region HexString-Bytes

        /// <summary> 每个字节都反转 </summary>
        /// <param name="HexValue">  </param>
        /// <param name="isReverse"> </param>
        /// <returns> </returns>
        public static byte[] HexStringToBytes(string HexValue, bool isReverse = false)
        {
            if (true)
            {
                HexValue = HexValue.Replace(" ", "");
                List<byte> bytedatas = new List<byte>();
                while (HexValue.Length > 0)
                {
                    string strtmp = HexValue.Length == 1 ? HexValue : HexValue.Substring(0, 2);
                    if (isReverse)
                    {
                        strtmp = ReverseString(strtmp);
                    }
                    bytedatas.Add(Convert.ToByte(Convert.ToUInt32(strtmp, 16)));
                    HexValue = HexValue.Length == 1 ? "" : HexValue.Substring(2);
                }
                return bytedatas.ToArray();
            }
            else
            {
                HexValue = Regex.Replace(HexValue, "[^0-9A-Fa-f]", "");
                if (HexValue.Length % 2 != 0)
                    HexValue = HexValue.Remove(HexValue.Length - 1, 1);
                if (HexValue.Length <= 0) return new byte[0];
                byte[] vBytes = new byte[HexValue.Length / 2];
                for (int i = 0; i < HexValue.Length; i += 2)
                    if (!byte.TryParse(HexValue.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                        vBytes[i / 2] = 0;
                return vBytes;
            }
        }

        public static string BytesToHexString(byte[] strASCII, bool isReverse = false, string separator = " ")
        {
            if (strASCII == null) return "";
            if (true)
            {
                byte[] tempData = strASCII;
                if (isReverse)
                {
                    tempData = strASCII.Reverse().ToArray();
                    return BitConverter.ToString(tempData, 0, tempData.Length).Replace("-", separator);
                }
                else
                {
                }

                return BitConverter.ToString(tempData, 0, tempData.Length).Replace("-", separator);
            }
            else
            {
                StringBuilder sbHex = new StringBuilder();

                foreach (byte chr in strASCII)
                {
                    string str = String.Format("{0:X2}", Convert.ToInt32(chr));
                    if (isReverse)
                    {
                        str = ReverseString(str);
                    }
                    sbHex.Append(str);
                    sbHex.Append(separator ?? string.Empty);
                }
                return sbHex.ToString();
            }
        }

        /// <summary> 每两个反转 </summary>
        /// <param name="HexValue"> </param>
        /// <returns> </returns>
        public static byte[] HexStringToBytesForBTWifi(string HexValue)
        {
            HexValue = HexValue.Replace(" ", "");
            List<byte> bytedatas = new List<byte>();
            List<byte> Reversebytedatas = new List<byte>();
            while (HexValue.Length > 0)
            {
                string strtmp = HexValue.Length == 1 ? HexValue : HexValue.Substring(0, 2);
                bytedatas.Add(Convert.ToByte(Convert.ToUInt32(strtmp, 16)));
                HexValue = HexValue.Length == 1 ? "" : HexValue.Substring(2);
            }
            int ilen = bytedatas.Count - 1;
            for (int i = 0; i < bytedatas.Count; i++)
            {
                Reversebytedatas.Add(bytedatas[ilen--]);
            }
            return Reversebytedatas.ToArray();
        }

        #endregion HexString-Bytes

        #region HexString-RealString

        public static string ConvertHexStringToRealString(string HexValue, string separator = null)
        {
            HexValue = string.IsNullOrEmpty(separator) ? HexValue : HexValue.Replace(string.Empty, separator);
            StringBuilder sbStrValue = new StringBuilder();
            while (HexValue.Length > 0)
            {
                sbStrValue.Append(Convert.ToChar(Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString());
                HexValue = HexValue.Substring(2);
            }
            return sbStrValue.ToString();
        }

        public static string ConvertRealStringToHexString(string strASCII, string separator = null)
        {
            StringBuilder sbHex = new StringBuilder();
            foreach (char chr in strASCII)
            {
                sbHex.Append(String.Format("{0:X2}", Convert.ToInt32(chr)));
                sbHex.Append(separator ?? string.Empty);
            }
            return sbHex.ToString();
        }

        #endregion HexString-RealString

        public static List<string> ConvertStringToList(string baseString, char splitChar)
        {
            List<string> strList = new List<string>();
            string[] arrTemp = baseString.Split(new char[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arrTemp)
            {
                strList.Add(item);
            }
            return strList;
        }

        public static string ReverseString(string original)
        {
            char[] chardata = original.ToCharArray();
            Array.Reverse(chardata);
            return new string(chardata);
        }

        /// <summary> 获取子字符串 </summary>
        /// <param name="BaseData">    父字符串 </param>
        /// <param name="SearchStart"> 从该字符串之后开始 </param>
        /// <param name="SearchEnd">   从该字符串之前结束,第一个匹配项 </param>
        /// <returns> </returns>
        public static string GetSubString(string BaseData, string SearchStart, string SearchEnd)
        {
            int index1 = 0, index2 = 0, index3 = 0;
            string ReturnData = "";

            try
            {
                index1 = BaseData.Length;
                index2 = BaseData.IndexOf(SearchStart);
                index3 = BaseData.IndexOf(SearchEnd);

                if (!string.IsNullOrEmpty(SearchStart) && !string.IsNullOrEmpty(SearchEnd))
                {
                    ReturnData = BaseData.Substring(index2 + SearchStart.Length, index3 - index2 - SearchStart.Length);
                }
                else if (string.IsNullOrEmpty(SearchEnd))
                {
                    ReturnData = BaseData.Substring(index2 + SearchStart.Length);
                }
                else if (string.IsNullOrEmpty(SearchStart))
                {
                    ReturnData = BaseData.Substring(0, index3);
                }
            }
            catch
            {
                return "";
            }

            return ReturnData;
        }
    }
}