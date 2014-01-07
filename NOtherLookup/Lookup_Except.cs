using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TValue> Except<TKey, TValue>(
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> comparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.Select(x => ValuesForKey(x, second, comparer)).ToLookup(comparer);
        }

        private static KeyValuePair<TKey, IEnumerable<TValue>> ValuesForKey<TKey, TValue>(
            IGrouping<TKey, TValue> current, IEnumerable<IGrouping<TKey, TValue>> second, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var excluded = second.Where(s => comparer.Equals(current.Key, s.Key)).SelectMany(s => s);
            return new KeyValuePair<TKey, IEnumerable<TValue>>(current.Key, current.Except(excluded));
        }
    }
}