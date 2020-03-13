using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using One.Core.ExtensionMethods;

namespace One.Core.Helper
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
