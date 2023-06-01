using System;
using System.Collections.Generic;

namespace One.Core.Models.Configs
{
    public class TestSecquenceConfigM : ICloneable
    {
        public string TestItem { get; set; }

        public string Description { get; set; }
        public string Customer { get; set; }
        public int Index { get; set; }

        public List<CommonConfigM> TestSecquenceItemConfigList { get; set; } = new List<CommonConfigM>();

        public object Clone()
        {
            TestSecquenceConfigM other = (TestSecquenceConfigM)this.MemberwiseClone();

            List<CommonConfigM> list = new List<CommonConfigM>();

            foreach (var item in TestSecquenceItemConfigList)
            {
                CommonConfigM temp = (CommonConfigM)item.Clone();
                list.Add(temp);
            }

            other.TestSecquenceItemConfigList = list;
            return other;
        }
    }
}