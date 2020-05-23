namespace One.Core.ExtensionMethods
{
    public static class ExtensionMethodsForByte
    {
        /// <summary> 转换当前byte[]为对应的string </summary>
        /// <param name="e"> string字符编码 </param>
        /// <returns> </returns>
        public static string ToString(this byte[] args, System.Text.Encoding encoding)
        {
            return encoding.GetString(args);
        }
    }
}