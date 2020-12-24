using System;
using System.ComponentModel;
using System.Linq;

namespace One.Core.ExtensionMethods
{
    public static class ExtensionMethodsForEnum
    {
        /// <summary> 获取枚举描述属性 </summary>
        /// <param name="value"> The value to act on. </param>
        /// <returns> The description attribute. </returns>
        public static string GetCustomAttributeDescription(this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return attr.Description;
        }

        /// <summary>
        /// 解析枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T TryParse<T>(this string value) where T : struct
        {
            var isSucc = Enum.TryParse<T>(value, out var result);

            if (!isSucc) return default(T);

            return result;
        }
    }
}