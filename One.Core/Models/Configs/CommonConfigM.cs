using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace One.Core.Models.Configs
{
    public class CommonConfigM : ICloneable
    {
        public CommonConfigM()
        {
        }

        [Editor("ReadOnly", "")]
        public string Name { get; set; }

        [Editor("ReadOnly", "")]
        public string Description { get; set; }

        [Editor("ReadOnly", "")]
        public string Type { get; set; }

        public object Value { get; set; }

        [Browsable(false)]
        public List<CommonConfigM> CommonConfigList { get; set; } = new List<CommonConfigM>();

        public object Clone()
        {
            CommonConfigM other = (CommonConfigM)this.MemberwiseClone();
            return other;
        }
    }
}