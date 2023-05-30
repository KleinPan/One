using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace One.Toolbox.Helpers
{
    internal class ByteHelper
    {
        public static string ByteToHexString(byte[] d, string s = " ", int len = -1)
        {
            if (len == -1)
                len = d.Length;
            return BitConverter.ToString(d, 0, len).Replace("-", s);
        }

        /// <summary> hex转byte </summary>
        /// <param name="mHex"> hex值 </param>
        /// <returns> 原始字符串 </returns>
        public static byte[] HexToByte(string mHex)
        {
            mHex = Regex.Replace(mHex, "[^0-9A-Fa-f]", "");
            if (mHex.Length % 2 != 0)
                mHex = mHex.Remove(mHex.Length - 1, 1);
            if (mHex.Length <= 0) return new byte[0];
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return vBytes;
        }

        /// <summary> byte转string </summary>
        /// <param name="mHex"> </param>
        /// <returns> </returns>
        public static string ByteToString(byte[] vBytes, int len = -1)
        {
            var br = from e in vBytes
                     where e != 0
                     select e;
            if (len == -1 || len > br.Count())
                len = br.Count();
            return Encoding.UTF8.GetString(br.Take(len).ToArray());
        }
    }
}