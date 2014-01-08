using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    internal static class Helpers
    {
        public static IEnumerable<TValue> ValuesForKey<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> lookup, TKey key, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            return lookup.Where(x => comparer.Equals(x.Key, key)).SelectMany(x => x);
        }

        public static ISet<TKey> Keys<TKey, TValue>(this ILookup<TKey, TValue> lookup, IEqualityComparer<TKey> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            return new HashSet<TKey>(lookup.Select(x => x.Key), comparer);
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