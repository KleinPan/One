using One.Core.Helpers;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace One.Core.ExtensionMethods
{
    /// <summary> 扩展字符串类 </summary>
    public static class ExtensionMethodsForString
    {
        #region 数据转换

        #region 转Int

        /// <summary> 转Int,失败返回0 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static int ToInt(this string t)
        {
            int n;
            if (!int.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary> 转Int,失败返回pReturn </summary>
        /// <param name="t">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static int ToInt(this string t, int pReturn)
        {
            int n;
            if (!int.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary> 是否是Int true:是 false:否 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static bool IsInt(this string t)
        {
            int n;
            return int.TryParse(t, out n);
        }

        #endregion 转Int

        #region 转Int16

        /// <summary> 转Int,失败返回0 </summary>
        /// <param name="e"> </param>
        /// <returns> </returns>
        public static Int16 ToInt16(this string t)
        {
            Int16 n;
            if (!Int16.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary> 转Int,失败返回pReturn </summary>
        /// <param name="e">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static Int16 ToInt16(this string t, Int16 pReturn)
        {
            Int16 n;
            if (!Int16.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary> 是否是Int true:是 false:否 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static bool IsInt16(this string t)
        {
            Int16 n;
            return Int16.TryParse(t, out n);
        }

        #endregion 转Int16

        #region 转byte

        /// <summary> 转byte,失败返回0 </summary>
        /// <param name="e"> </param>
        /// <returns> </returns>
        public static byte Tobyte(this string t)
        {
            byte n;
            if (!byte.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary> 转byte[] </summary>
        /// <param name="e"> </param>
        /// <returns> </returns>
        public static byte[] ToByte(this string t, System.Text.Encoding encoding)
        {
            return encoding.GetBytes(t);
        }

        /// <summary> 转byte,失败返回pReturn </summary>
        /// <param name="e">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static byte Tobyte(this string t, byte pReturn)
        {
            byte n;
            if (!byte.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary> 是否是byte true:是 false:否 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static bool Isbyte(this string t)
        {
            byte n;
            return byte.TryParse(t, out n);
        }

        #endregion 转byte

        #region 转Long

        /// <summary> 转Long,失败返回0 </summary>
        /// <param name="e"> </param>
        /// <returns> </returns>
        public static long ToLong(this string t)
        {
            long n;
            if (!long.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary> 转Long,失败返回pReturn </summary>
        /// <param name="e">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static long ToLong(this string t, long pReturn)
        {
            long n;
            if (!long.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary> 是否是Long true:是 false:否 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static bool IsLong(this string t)
        {
            long n;
            return long.TryParse(t, out n);
        }

        #endregion 转Long

        #region 转Double

        /// <summary> 转Int,失败返回0 </summary>
        /// <param name="e"> </param>
        /// <returns> </returns>
        public static double ToDouble(this string t)
        {
            double n;
            if (!double.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary> 转Double,失败返回pReturn </summary>
        /// <param name="e">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static double ToDouble(this string t, double pReturn)
        {
            double n;
            if (!double.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary> 是否是Double true:是 false:否 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static bool IsDouble(this string t)
        {
            double n;
            return double.TryParse(t, out n);
        }

        #endregion 转Double

        #region 转Decimal

        /// <summary> 转Decimal,失败返回0 </summary>
        /// <param name="e"> </param>
        /// <returns> </returns>
        public static decimal ToDecimal(this string t)
        {
            decimal n;
            if (!decimal.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary> 转Decimal,失败返回pReturn </summary>
        /// <param name="e">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static decimal ToDecimal(this string t, decimal pReturn)
        {
            decimal n;
            if (!decimal.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary> 是否是Decimal true:是 false:否 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static bool IsDecimal(this string t)
        {
            decimal n;
            return decimal.TryParse(t, out n);
        }

        #endregion 转Decimal

        #region 转DateTime

        /// <summary> 转DateTime,失败返回当前时间 </summary>
        /// <param name="e"> </param>
        /// <returns> </returns>
        public static DateTime ToDateTime(this string t)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return DateTime.Now;
            return n;
        }

        /// <summary> 转DateTime,失败返回pReturn </summary>
        /// <param name="e">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static DateTime ToDateTime(this string t, DateTime pReturn)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary> 转DateTime,失败返回pReturn </summary>
        /// <param name="e">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static string ToDateTime(this string t, string pFormat, string pReturn)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return pReturn;
            return n.ToString(pFormat);
        }

        /// <summary> 转DateTime,失败返回空 </summary>
        /// <param name="e">       </param>
        /// <param name="pReturn"> 失败返回的值 </param>
        /// <returns> </returns>
        public static string ToDateTime(this string t, string pFormat)
        {
            return t.ToDateTime(pFormat, string.Empty);
        }

        public static string ToShortDateTime(this string t)
        {
            return t.ToDateTime("yyyy-MM-dd", string.Empty);
        }

        /// <summary> 是否是DateTime true:是 false:否 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static bool IsDateTime(this string t)
        {
            DateTime n;
            return DateTime.TryParse(t, out n);
        }

        #endregion 转DateTime

        #region 与int[]相关

        /// <summary> 转int[],字符串以逗号(,)隔开,请确保字符串内容都合法,否则会出错 </summary>
        /// <param name="pStr"> </param>
        /// <returns> </returns>
        public static int[] ToIntArr(this string t)
        {
            return t.ToIntArr(new char[] { ',' });
        }

        /// <summary> 转int[],字符串以逗号(,)隔开,请确保字符串内容都合法,否则会出错 </summary>
        /// <param name="t">      </param>
        /// <param name="pSplit"> 隔开的 </param>
        /// <returns> </returns>
        public static int[] ToIntArr(this string t, char[] pSplit)
        {
            if (t.Length == 0)
            {
                return new int[] { };
            }

            string[] ArrStr = t.Split(pSplit, StringSplitOptions.None);
            int[] iStr = new int[ArrStr.Length];

            for (int i = 0; i < ArrStr.Length; i++)
                iStr[i] = int.Parse(ArrStr[i]);

            return iStr;
        }

        #endregion 与int[]相关

        #region 过滤字符串的非int,重新组合成字符串

        /// <summary> 过滤字符串的非int,重新组合成字符串 </summary>
        /// <param name="t">      </param>
        /// <param name="pSplit"> 分隔符 </param>
        /// <returns> </returns>
        public static string ClearNoInt(this string t, char pSplit)
        {
            string sStr = string.Empty;
            string[] ArrStr = t.Split(pSplit);

            for (int i = 0; i < ArrStr.Length; i++)
            {
                string lsStr = ArrStr[i];

                if (lsStr.IsInt())
                    sStr += lsStr + pSplit;
                else
                    continue;
            }

            if (sStr.Length > 0)
                sStr = sStr.TrimEnd(pSplit);

            return sStr;
        }

        /// <summary> 过滤字符串的非int,重新组合成字符串 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static string ClearNoInt(this string t)
        {
            return t.ClearNoInt(',');
        }

        #endregion 过滤字符串的非int,重新组合成字符串

        #region 是否可以转换成int[]

        /// <summary> 是否可以转换成int[],true:是,false:否 </summary>
        /// <param name="t">      </param>
        /// <param name="pSplit"> 分隔符 </param>
        /// <returns> </returns>
        public static bool IsIntArr(this string t, char pSplit)
        {
            string[] ArrStr = t.Split(pSplit);
            bool b = true;

            for (int i = 0; i < ArrStr.Length; i++)
            {
                if (!ArrStr[i].IsInt())
                {
                    b = false;
                    break;
                }
            }

            return b;
        }

        /// <summary> 是否可以转换成int[],true:是,false:否 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static bool IsIntArr(this string t)
        {
            return t.IsIntArr(',');
        }

        #endregion 是否可以转换成int[]

        #endregion 数据转换

        #region 载取左字符

        /// <summary> 载取左字符 </summary>
        /// <param name="t">       </param>
        /// <param name="pLen">    字符个数 </param>
        /// <param name="pReturn"> 超出时后边要加的返回的内容 </param>
        /// <returns> </returns>
        public static string Left(this string t, int pLen, string pReturn)
        {
            if (t == null || t.Length == 0)
                return string.Empty;
            pLen *= 2;
            int i = 0, j = 0;
            foreach (char c in t)
            {
                if (c > 127)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i > pLen)
                {
                    return t.Substring(0, j) + pReturn;
                }

                j++;
            }

            return t;
        }

        public static string Left(this string t, int pLen)
        {
            return Left(t, pLen, string.Empty);
        }

        public static string StrLeft(this string t, int pLen)
        {
            if (t == null)
            {
                return "";
            }
            if (t.Length > pLen)
            {
                return t.Substring(0, pLen);
            }
            return t;
        }

        #endregion 载取左字符

        #region 删除文件名或路径的特殊字符

        private class ClearPathUnsafeList
        {
            public static readonly string[] unSafeStr = { "/", "\\", ":", "*", "?", "\"", "<", ">", "|" };
        }

        /// <summary> 删除文件名或路径的特殊字符 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static string ClearPathUnsafe(this string t)
        {
            foreach (string s in ClearPathUnsafeList.unSafeStr)
            {
                t = t.Replace(s, "");
            }

            return t;
        }

        #endregion 删除文件名或路径的特殊字符

        #region 字符串真实长度 如:一个汉字为两个字节

        /// <summary> 字符串真实长度 如:一个汉字为两个字节 </summary>
        /// <param name="s"> </param>
        /// <returns> </returns>
        public static int LengthReal(this string s)
        {
            return Encoding.Default.GetBytes(s).Length;
        }

        #endregion 字符串真实长度 如:一个汉字为两个字节

        #region 去除小数位最后为0的

        /// <summary> 去除小数位最后为0的 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static decimal ClearDecimal0(this string t)
        {
            decimal d;
            if (decimal.TryParse(t, out d))
            {
                return decimal.Parse(double.Parse(d.ToString("g")).ToString());
            }
            return 0;
        }

        #endregion 去除小数位最后为0的

        #region 进制转换

        /// <summary> 16进制转二进制 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static string Change16To2(this string t)
        {
            String BinOne = string.Empty;
            String BinAll = string.Empty;
            char[] nums = t.ToCharArray();
            for (int i = 0; i < nums.Length; i++)
            {
                string number = nums[i].ToString();
                int num = Int32.Parse(number, System.Globalization.NumberStyles.HexNumber);

                BinOne = Convert.ToString(num, 2).PadLeft(4, '0');
                BinAll = BinAll + BinOne;
            }
            return BinAll;
        }

        /// <summary> 二进制转十进制 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static Int64 Change2To10(this string t)
        {
            char[] arrc = t.ToCharArray();
            Int64 all = 0, indexC = 1;
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                if (arrc[i] == '1')
                {
                    all += indexC;
                }
                indexC = indexC * 2;
            }

            return all;
        }

        /// <summary> 二进制转换byte[]数组 </summary>
        /// <param name="s"> </param>
        /// <returns> </returns>
        public static byte[] Change2ToBytes(this string t)
        {
            List<byte> list = new List<byte>();

            char[] arrc = t.ToCharArray();
            byte n = 0;
            char c;
            int j = 0;
            //倒序获取位
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                c = arrc[i];

                if (c == '1')
                {
                    n += Convert.ToByte(Math.Pow(2, j));
                }
                j++;

                if (j % 8 == 0)
                {
                    list.Add(n);
                    j = 0;
                    n = 0;
                }
            }

            //剩余最高位
            if (n > 0)
                list.Add(n);

            byte[] arrb = new byte[list.Count];

            int j1 = 0;
            //倒序
            for (int i = list.Count - 1; i >= 0; i--)
            {
                arrb[j1] = list[i];
                j1++;
            }
            return arrb;
        }

        /// <summary> 二进制转化为索引id数据,从右到左 </summary>
        /// <param name="t"> </param>
        /// <returns> </returns>
        public static int[] Change2ToIndex(this string t)
        {
            List<int> list = new List<int>();
            char[] arrc = t.ToCharArray();
            char c;
            int j = 0;

            //倒序获取位
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                j++;
                c = arrc[i];

                if (c == '1')
                {
                    list.Add(j);
                }
            }

            return list.ToArray();
        }

        #endregion 进制转换

        /*

        #region html url编码 解码

        /// <summary> Html Encode </summary>
        /// <param name="pStr"> </param>
        /// <returns> </returns>
        public static string HtmlEncode(this string t)
        {
            return HttpContext.Current.Server.HtmlEncode(t);
        }

        /// <summary> Html Decode </summary>
        /// <param name="pStr"> </param>
        /// <returns> </returns>
        public static string HtmlDecode(this string t)
        {
            return HttpContext.Current.Server.HtmlDecode(t);
        }

        /// <summary> URL Encode </summary>
        /// <param name="pStr"> </param>
        /// <returns> </returns>
        public static string URLEncode(this string t)
        {
            return HttpContext.Current.Server.UrlEncode(t);
        }

        /// <summary> URL Decode </summary>
        /// <param name="pStr"> </param>
        /// <returns> </returns>
        public static string URLDecode(this string t)
        {
            return HttpContext.Current.Server.UrlDecode(t);
        }

        #endregion html url编码 解码

        #region 向客户端输出内容

        /// <summary> 向客户端输出内容 </summary>
        /// <param name="t"> </param>
        public static void Write(this string t)
        {
            HttpContext.Current.Response.Write(t);
        }

        /// <summary> 向客户端输出内容 </summary>
        /// <param name="t"> </param>
        public static void WriteLine(this string t)
        {
            HttpContext.Current.Response.Write(t + "<br />");
        }

        #endregion 向客户端输出内容

        */

        /// <summary> 验证字符串是否由正负号（+-）、数字、小数点构成，并且最多只有一个小数点 </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static bool IsNumeric(this string str)
        {
            Regex regex = new Regex(@"^[+-]?\d+[.]?\d*$");
            return regex.IsMatch(str);
        }

        /// <summary> 验证字符串是否仅由[0-9]构成 </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static bool IsNumericOnly(this string str)
        {
            Regex regex = new Regex("[0-9]");
            return regex.IsMatch(str);
        }

        /// <summary> 验证字符串是否由字母和数字构成 </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static bool IsNumericOrLetters(this string str)
        {
            Regex regex = new Regex("[a-zA-Z0-9]");
            return regex.IsMatch(str);
        }

        /// <summary> 验证是否为空字符串。若无需裁切两端空格，建议直接使用 String.IsNullOrEmpty(string) </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        /// <remarks> 不同于String.IsNullOrEmpty(string)，此方法会增加一步Trim操作。如 IsNullOrEmptyStr(" ") 将返回 true。 </remarks>
        public static bool IsNullOrEmptyStr(this string str)
        {
            if (string.IsNullOrEmpty(str)) { return true; }
            if (str.Trim().Length == 0) { return true; }
            return false;
        }

        /// <summary> 裁切字符串（中文按照两个字符计算） </summary>
        /// <param name="str">        旧字符串 </param>
        /// <param name="len">        新字符串长度 </param>
        /// <param name="HtmlEnable"> 为 false 时过滤 Html 标签后再进行裁切，反之则保留 Html 标签。 </param>
        /// <remarks>
        /// <para> 注意： <ol> <li> 若字符串被截断则会在末尾追加“...”，反之则直接返回原始字符串。 </li><li> 参数 <paramref name="HtmlEnable"/> 为 false 时会先调用 <see cref="uoLib.Common.Functions.HtmlFilter"/> 过滤掉 Html 标签再进行裁切。 </li><li> 中文按照两个字符计算。若指定长度位置恰好只获取半个中文字符，则会将其补全，如下面的例子： <br/>
        /// <code>
        ///<![CDATA[
        ///string str = "感谢使用uoLib。";
        ///string A = CutStr(str,4);   // A = "感谢..."
        ///string B = CutStr(str,5);   // B = "感谢使..."
        ///]]>
        /// </code>
        /// </li></ol>
        /// </para>
        /// </remarks>
        public static string CutStr(this string str, int len, bool HtmlEnable)
        {
            if (str == null || str.Length == 0 || len <= 0) { return string.Empty; }

            if (HtmlEnable == false) str = HTMLHelper.HtmlFilter(str);
            int l = str.Length;

            #region 计算长度

            int clen = 0;//当前长度
            while (clen < len && clen < l)
            {
                //每遇到一个中文，则将目标长度减一。
                if ((int)str[clen] > 128) { len--; }
                clen++;
            }

            #endregion 计算长度

            if (clen < l)
            {
                return str.Substring(0, clen) + "...";
            }
            else
            {
                return str;
            }
        }

        /// <summary> 裁切字符串（中文按照两个字符计算，裁切前会先过滤 Html 标签） </summary>
        /// <param name="str"> 旧字符串 </param>
        /// <param name="len"> 新字符串长度 </param>
        /// <remarks>
        /// <para> 注意： <ol> <li> 若字符串被截断则会在末尾追加“...”，反之则直接返回原始字符串。 </li><li> 中文按照两个字符计算。若指定长度位置恰好只获取半个中文字符，则会将其补全，如下面的例子： <br/>
        /// <code>
        ///<![CDATA[
        ///string str = "感谢使用uoLib模块。";
        ///string A = CutStr(str,4);   // A = "感谢..."
        ///string B = CutStr(str,5);   // B = "感谢使..."
        ///]]>
        /// </code>
        /// </li></ol>
        /// </para>
        /// </remarks>
        public static string CutStr(this string str, int len)
        {
            if (IsNullOrEmptyStr(str)) { return string.Empty; }
            else
            {
                return CutStr(str, len, false);
            }
        }

        /// <summary> 获取字符串长度。与string.Length不同的是，该方法将中文作 2 个字符计算。 </summary>
        /// <param name="str"> 目标字符串 </param>
        /// <returns> </returns>
        public static int GetLength(this string str)
        {
            if (str == null || str.Length == 0) { return 0; }

            int l = str.Length;
            int realLen = l;

            #region 计算长度

            int clen = 0;//当前长度
            while (clen < l)
            {
                //每遇到一个中文，则将实际长度加一。
                if ((int)str[clen] > 128) { realLen++; }
                clen++;
            }

            #endregion 计算长度

            return realLen;
        }

        /// <summary> 将形如 10.1MB 格式对用户友好的文件大小字符串还原成真实的文件大小，单位为字节。 </summary>
        /// <param name="formatedSize"> 形如 10.1MB 格式的文件大小字符串 </param>
        /// <remarks> 参见： <see cref="uoLib.Common.Functions.FormatFileSize(long)"/> </remarks>
        /// <returns> </returns>
        public static long GetFileSizeFromString(this string formatedSize)
        {
            if (IsNullOrEmptyStr(formatedSize)) throw new ArgumentNullException("formatedSize");

            long size;
            if (long.TryParse(formatedSize, out size)) return size;

            //去掉数字分隔符
            formatedSize = formatedSize.Replace(",", "");

            Regex re = new Regex(@"^([\d\.]+)((?:TB|GB|MB|KB|Bytes))$");
            if (re.IsMatch(formatedSize))
            {
                MatchCollection mc = re.Matches(formatedSize);
                Match m = mc[0];
                double s = double.Parse(m.Groups[1].Value);

                switch (m.Groups[2].Value)
                {
                    case "TB":
                        s *= 1099511627776;
                        break;

                    case "GB":
                        s *= 1073741824;
                        break;

                    case "MB":
                        s *= 1048576;
                        break;

                    case "KB":
                        s *= 1024;
                        break;
                }

                size = (long)s;
                return size;
            }

            throw new ArgumentException("formatedSize");
        }

        public static string GetJsSafeStr(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return str.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        public static string Encode(this string content, Encoding encode = null)
        {
            if (encode == null) return content;

            return System.Web.HttpUtility.UrlEncode(content, Encoding.UTF8);
        }

        public static string ToOtherEncoding(this string content, Encoding encode = null)
        {
            if (encode == null) return content;

            //返回转换后的字符
            return encode.GetString(encode.GetBytes(content));
        }
    }
}