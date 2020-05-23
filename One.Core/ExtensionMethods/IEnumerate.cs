﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace One.Core.ExtensionMethods
{
    public static class ExtensionMethodsForEnumerate
    {
        /// <summary> 返回指定筛选条件下序列非重复元素的首项 </summary>
        /// <typeparam name="TSource"> </typeparam>
        /// <typeparam name="TKey"> </typeparam>
        /// <param name="source">      </param>
        /// <param name="keySelector"> </param>
        /// <returns> </returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                //不存在则返回true
                if (seenKeys.Add(keySelector(element)))
                {
                    //迭代器返回给输出返回值
                    yield return element;
                }
            }
        }

        public static int FindIndex<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (var item in collection)
            {
                if (predicate(item))
                    return i;
                i++;
            }

            return -1;
        }

        /// <summary> Enumerates for each in this collection. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="this">   The @this to act on. </param>
        /// <param name="action"> The action. </param>
        /// <returns> An enumerator that allows foreach to be used to process for each in this collection. </returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            T[] array = @this.ToArray();
            foreach (T t in array)
            {
                action(t);
            }
            return array;
        }
    }
}