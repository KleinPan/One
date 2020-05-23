using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace One.Core.ExtensionMethods
{
    public static class ExtensionMethodsForCollection
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }

        public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }
    }
}