using System;

namespace One.Core.Attributes
{
    /// <summary> 自定义特性 </summary>
    public class CustomerAttribute : Attribute
    {
        public string Customer { get; set; }

        public CustomerAttribute(string CustomerEnum)
        {
            Customer = CustomerEnum;
        }
    }
}