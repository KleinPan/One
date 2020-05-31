using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace One.Core.Helper
{
   public class AssemblyHelper
    {

        public static Attribute GetAttribute(Assembly assembly, Type attributeType)
        {
            object[] attributes = assembly.GetCustomAttributes(attributeType, false);
            if (attributes.Length == 0)
            {
                return null;
            }

            return (Attribute)attributes[0];
        }
    }
}
