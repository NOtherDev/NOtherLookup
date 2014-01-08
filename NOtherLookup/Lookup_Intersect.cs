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
            IEnumerable<IGrouping<TKey, TValue>> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> comparer)
        {
            var secondKeys = new HashSet<TKey>(second.Select(x => x.Key), comparer);
            return first.Where(g => secondKeys.Remove(g.Key)).Select(g => IntersectValuesForKey(g, second, comparer));
        }

        private static KeyValuePair<TKey, IEnumerable<TValue>> IntersectValuesForKey<TKey, TValue>(
            IGrouping<TKey, TValue> current, IEnumerable<IGrouping<TKey, TValue>> second, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var matching = second.Where(x => comparer.Equals(x.Key, current.Key)).SelectMany(x => x);
            return new KeyValuePair<TKey, IEnumerable<TValue>>(current.Key, current.Intersect(matching));
        }
    }
}