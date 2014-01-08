using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TResult> Join<TKey, TOuter, TInner, TResult>(
            this ILookup<TKey, TOuter> outer, ILookup<TKey, TInner> inner, 
            Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer = null)
        {
            if (outer == null)
                throw new ArgumentNullException("outer");
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return outer.SelectMany(o => o, (o, e) => Join_ValuesForKey(o.Key, e, inner, resultSelector, comparer))
                .ToLookup(comparer);
        }

        private static KeyValuePair<TKey, IEnumerable<TResult>> Join_ValuesForKey<TKey, TOuter, TInner, TResult>(
            TKey key, TOuter current, IEnumerable<IGrouping<TKey, TInner>> inner,
            Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return Pair.Of(key, inner.ValuesForKey(key, comparer).Select(x => resultSelector(current, x)));
        }
    }
}