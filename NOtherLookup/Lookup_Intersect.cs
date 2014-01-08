using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TValue> Intersect<TKey, TValue>(
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> comparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            return IntersectImpl(first, second, comparer).ToLookup(comparer);
        }
        
        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> IntersectImpl<TKey, TValue>(
            ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> comparer)
        {
            var firstKeys = first.Keys(comparer);
            var secondKeys = second.Keys(comparer);

            return firstKeys.Where(x => secondKeys.Remove(x))
                .Select(x => Intersect_ValuesForKey(x, first.ValuesForKey(x, comparer), second, comparer));
        }

        private static KeyValuePair<TKey, IEnumerable<TValue>> Intersect_ValuesForKey<TKey, TValue>(
            TKey key, IEnumerable<TValue> current, IEnumerable<IGrouping<TKey, TValue>> second, IEqualityComparer<TKey> comparer)
        {
            return Pair.Of(key, current.Intersect(second.ValuesForKey(key, comparer)));
        }
    }
}