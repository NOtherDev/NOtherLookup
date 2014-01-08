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
    }
}