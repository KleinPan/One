using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One.Core.ExtensionMethods
{
    /// <summary>数组扩展方法</summary>
    public static class ExtensionMethodsForArray
    {
        /// <summary>复制指定长度的字符到新的字符串</summary>
        /// <param name="str">   </param>
        /// <param name="start"> </param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string[] Copy(this Array str, int start, int length)
        {
            if (length > str.Length)
            {
                return null;
            }

            string[] c = new string[length];
            for (int i = 0; i < length; i++)
            {
                c[i] += str.GetValue(start + i);
            }
            return c;
        }

        /// <summary>
        /// Array添加单项
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="array">Array</param>
        /// <param name="item">需要添加项</param>
        /// <returns>返回新的Array</returns>
        public static T[] Add<T>(this T[] array, T item)
        {
            int _count = array.Length;
            Array.Resize<T>(ref array, _count + 1);
            array[_count] = item;
            return array;
        }
        /// <summary>
        /// Array添加多项
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sourceArray">Array</param>
        /// <param name="addArray">Array</param>
        /// <returns>返回新的Array</returns>
        public static T[] AddRange<T>(this T[] sourceArray, T[] addArray)
        {
            int _count = sourceArray.Length;
            int _addCount = addArray.Length;
            Array.Resize<T>(ref sourceArray, _count + _addCount);
            //foreach (T t in addArray)
            //{
            //  sourceArray[_count] = t;
            //  _count++;
            //}
            addArray.CopyTo(sourceArray, _count);
            return sourceArray;
        }
        /// <summary>
        /// Array添加
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="array">Array</param>
        /// <param name="item">需要添加项</param>
        /// <returns>返回新的Array</returns>
        public static T[] AddLinq<T>(this T[] array, T item)
        {
            array = array.Concat<T>(new T[1] { item }).ToArray();
            return array;
        }
        /// <summary>
        /// Array添加
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sourceArray">Array</param>
        /// <param name="addArray">Array</param>
        /// <returns>返回新的Array</returns>
        public static T[] AddRangeLinq<T>(this T[] sourceArray, T[] addArray)
        {
            sourceArray = sourceArray.Concat<T>(addArray).ToArray();
            return sourceArray;
        }
    }
}
