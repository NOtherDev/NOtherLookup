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

            return JoinImpl(outer, inner, resultSelector, comparer).ToLookup(comparer);
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TResult>>> JoinImpl<TKey, TOuter, TInner, TResult>(
            IEnumerable<IGrouping<TKey, TOuter>> outer, ILookup<TKey, TInner> inner, 
            Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            foreach (var outerGrouping in outer)
            {
                foreach (var outerElement in outerGrouping)
                {
                    yield return JoinValuesForKey(outerGrouping.Key, outerElement, inner, resultSelector, comparer);
                }
            }
        }

        private static KeyValuePair<TKey, IEnumerable<TResult>> JoinValuesForKey<TKey, TOuter, TInner, TResult>(
            TKey key, TOuter current, IEnumerable<IGrouping<TKey, TInner>> inner,
            Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var values = inner.Where(x => comparer.Equals(x.Key, key)).SelectMany(x => x).Select(x => resultSelector(current, x));
            return new KeyValuePair<TKey, IEnumerable<TResult>>(key, values);
        }
    }
}