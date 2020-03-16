using System;
using System.Diagnostics;
using System.Reflection;

namespace One.Core.Helper

{
    /// <summary> 反射帮助类 </summary>
    public class ReflectionHelper
    {
        /// <summary> 遍历类属性 </summary>
        /// <typeparam name="T">  </typeparam>
        /// <param name="model">  </param>
        public static void ForeachClassProperties<T>(T model)
        {
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                string PropertyName = item.Name;
                object PropertyValue = item.GetValue(model, null);
                //EsspClassLibrary.Message.Mes.Notify(PropertyName + "：" + PropertyValue.ToString() + "\r\n");
                Console.WriteLine(PropertyName + "：" + PropertyValue.ToString() + "\r\n");
            }
        }

        /// <summary> 遍历类字段 </summary>
        /// <typeparam name="T">  </typeparam>
        /// <param name="model">  </param>
        public static void ForeachClassFields<T>(T model)
        {
            Type t = model.GetType();
            FieldInfo[] PropertyList = t.GetFields();
            foreach (FieldInfo item in PropertyList)
            {
                string FieldName = item.Name;
                object FieldValue = item.GetValue(model);
                //EsspClassLibrary.Message.Mes.Notify(FieldName + "：" + FieldValue.ToString() + "\r\n");
                Console.WriteLine(FieldName + "：" + FieldValue.ToString() + "\r\n");
            }
        }

        /// <summary> 获取父方法名 </summary>
        /// <returns>  </returns>
        public static string GetParentMethodName()
        {
            string str = "";

            StackTrace ss = new StackTrace(true);
            MethodBase mb = ss.GetFrame(1).GetMethod();
            //取得父方法命名空间
            //str += mb.DeclaringType.Namespace + "\n";
            //取得父方法类名
            //str += mb.DeclaringType.Name + "\n";
            //取得父方法类全名
            //str += mb.DeclaringType.FullName + "\n";
            //取得父方法名
            //str += mb.Name + "\n";

            str += mb.Name;

            return str;
        }
    }
}