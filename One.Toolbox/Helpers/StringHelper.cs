using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace One.Toolbox.Helpers
{
    internal class StringHelper
    {
        /// <summary> 字符串转hex值 </summary>
        /// <param name="str">   字符串 </param>
        /// <param name="space"> 间隔符号 </param>
        /// <returns> 结果 </returns>
        public static string StringToHex(string str, string space)
        {
            return BitConverter.ToString(Encoding.UTF8.GetBytes(str)).Replace("-", space);
        }

        /// <summary> hex值转字符串 </summary>
        /// <param name="mHex"> hex值 </param>
        /// <returns> 原始字符串 </returns>
        public static string HexToString(string mHex)
        {
            mHex = Regex.Replace(mHex, "[^0-9A-Fa-f]", "");
            if (mHex.Length % 2 != 0)
                mHex = mHex.Remove(mHex.Length - 1, 1);
            if (mHex.Length <= 0) return "";
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return Encoding.UTF8.GetString(vBytes);
        }
    }
}