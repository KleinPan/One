using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using static System.Net.Mime.MediaTypeNames;

namespace One.Core.Helpers
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

        public Assembly assembly;

        public AssemblyHelper()
        {
            assembly = Assembly.GetExecutingAssembly();

            FileVersionInfo = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0] as AssemblyFileVersionAttribute;
            CompanyInfo = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0] as AssemblyCompanyAttribute;
            ProductInfo = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0] as AssemblyProductAttribute;
            TitleInfo = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute;
            CopyrightInfo = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute;
            DescriptionInfo = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute;
        }

        /// <summary> 产品版本信息 </summary>
        public string ProductVersion
        {
            get;
            private set;
        }

        /// <summary> 文件版本信息 </summary>
        public AssemblyFileVersionAttribute FileVersionInfo
        {
            get;
            private set;
        }

        /// <summary> 公司信息 </summary>
        public AssemblyCompanyAttribute CompanyInfo
        {
            get;
            private set;
        }

        /// <summary> 产品信息 </summary>
        public AssemblyProductAttribute ProductInfo
        {
            get;
            private set;
        }

        /// <summary> 标题信息 </summary>
        public AssemblyTitleAttribute TitleInfo
        {
            get;
            private set;
        }

        /// <summary> 版权信息 </summary>
        public AssemblyCopyrightAttribute CopyrightInfo
        {
            get;
            private set;
        }

        /// <summary> 描述信息 </summary>
        public AssemblyDescriptionAttribute DescriptionInfo
        {
            get;
            private set;
        }
    }
}