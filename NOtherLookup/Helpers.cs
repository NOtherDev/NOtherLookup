using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    internal static class Helpers
    {
        private static IEqualityComparer<TKey> NotNullComparer<TKey>(IEqualityComparer<TKey> comparer)
        {
            return comparer ?? EqualityComparer<TKey>.Default;
        }

        public static IEnumerable<TValue> ValuesForKey<TKey, TValue>(
            this IEnumerable<IGrouping<TKey, TValue>> lookup, TKey key, IEqualityComparer<TKey> comparer)
        {
            return lookup.Where(x => NotNullComparer(comparer).Equals(x.Key, key)).SelectMany(x => x);
        }

        public static ISet<TKey> Keys<TKey, TValue>(
            this ILookup<TKey, TValue> lookup, IEqualityComparer<TKey> comparer = null)
        {
            return new HashSet<TKey>(lookup.Select(x => x.Key), NotNullComparer(comparer));
        }
    }

    internal static class Pair
    {
        public static KeyValuePair<TKey, IEnumerable<TValue>> Of<TKey, TValue>(TKey key, IEnumerable<TValue> value)
        {
            return new KeyValuePair<TKey, IEnumerable<TValue>>(key, value);
        }
    }
}