using System;
using System.Collections.Generic;
using System.Text;

namespace One.Core.PathHelper
{
  public   class PathHelper
    {
        /// <summary>
        /// 获取父目录
        /// </summary>
        /// <param name="path">当前目录</param>
        /// <param name="i">第几级父目录</param>
        /// <returns></returns>
        public static string GetParentDirectory(string path, int i = 1)
        {
            string result = "";
            if (i == 1)
            {
                result = path.Substring(0, path.LastIndexOf(@"\"));
            }
            else
            {
                var temp = path.Split("\\");
                string temp2 = "";
                for (int j = 0; j < temp.Length - i; j++)
                {
                    temp2 += temp[j] + "\\";
                }

                result = temp2.TrimEnd('\\');
            }

            return result;

        }

    }

  
}
