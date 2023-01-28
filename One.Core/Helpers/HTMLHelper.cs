using One.Core.ExtensionMethods;

using System.Text.RegularExpressions;

namespace One.Core.Helpers
{
    public class HTMLHelper
    {
        public static string HtmlFilter(string str)
        {
            if (str.IsNullOrEmptyStr())
            {
                return string.Empty;
            }
            Regex regex = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return regex.Replace(str, "");
        }
    }
}