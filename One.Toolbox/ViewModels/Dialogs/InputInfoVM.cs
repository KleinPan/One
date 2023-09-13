using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aquarius.Common.Models
{
    public class InputInfoVM
    {
        /// <summary> 唯一标识符，获取时候传入 </summary>
        public string Key { get; set; }

        /// <summary> 标题 </summary>
        public string Title { get; set; }

        /// <summary> 描述 </summary>
        public string Descrption { get; set; }

        public int DataLength { get; set; }

        /// <summary> 正则校验规则 </summary>
        public Regex DataRule { get; set; }

        public InputInfoVM(string title, string key)
        {
            Title = title;
            Key = key;
        }
    }
}