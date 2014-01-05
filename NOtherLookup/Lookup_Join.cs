using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        public static ILookup<TKey, TResult> Join<TKey, TOuter, TInner, TResult>(this ILookup<TKey, TOuter> outer,
            ILookup<TKey, TInner> inner, Func<TOuter, TInner, TResult> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException("outer");
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return outer.JoinImpl(inner, resultSelector).ToLookup();
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TResult>>> JoinImpl<TKey, TOuter, TInner, TResult>(
            this ILookup<TKey, TOuter> outer,
            ILookup<TKey, TInner> inner, 
            Func<TOuter, TInner, TResult> resultSelector)
        {
            foreach (var outerGrouping in outer)
            {
                foreach (var outerElement in outerGrouping)
                {
                    var key = outerGrouping.Key;
                    var element = outerElement;
                    var values = inner[key].Select(innerElement => resultSelector(element, innerElement));
                    yield return new KeyValuePair<TKey, IEnumerable<TResult>>(key, values);
                }
            }
        }
    }
}