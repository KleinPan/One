using System;

namespace One.Core.Attributes
{
    public class ExportAttribute : Attribute
    {
        public bool Export { get; set; }

        public ExportAttribute(bool color = true)
        {
            Export = color;
        }
    }
}