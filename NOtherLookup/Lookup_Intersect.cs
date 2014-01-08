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
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> keyComparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            return IntersectImpl(first, second, keyComparer).ToLookup(keyComparer);
        }
        
        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> IntersectImpl<TKey, TValue>(
            ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> keyComparer)
        {
            var firstKeys = first.Keys(keyComparer);
            var secondKeys = second.Keys(keyComparer);

            return firstKeys.Where(x => secondKeys.Remove(x))
                .Select(x => Intersect_ValuesForKey(x, first.ValuesForKey(x, keyComparer), second, keyComparer));
        }

        private static KeyValuePair<TKey, IEnumerable<TValue>> Intersect_ValuesForKey<TKey, TValue>(
            TKey key, IEnumerable<TValue> current, IEnumerable<IGrouping<TKey, TValue>> second, IEqualityComparer<TKey> keyComparer)
        {
            return Pair.Of(key, current.Intersect(second.ValuesForKey(key, keyComparer)));
        }
    }
}