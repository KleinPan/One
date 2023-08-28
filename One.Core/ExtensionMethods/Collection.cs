using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace One.Core.ExtensionMethods
{
    public static class ExtensionMethodsForCollection
    {
        /// <summary> 添加T类型的序列到当前序列末尾（带通知） </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="collection"> </param>
        /// <param name="items">      </param>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }

        /// <summary> 添加T类型的序列到当前序列末尾 </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="collection"> </param>
        /// <param name="items">      </param>
        public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }
    }
}