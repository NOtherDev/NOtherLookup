using System;
using System.Collections.Generic;
using System.Linq;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        public static ILookup<TKey, TResult> Select<TKey, TValue, TResult>(this ILookup<TKey, TValue> lookup, Func<IEnumerable<TValue>, IEnumerable<TResult>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            return lookup
                .Select(x => new KeyValuePair<TKey, IEnumerable<TResult>>(x.Key, selector(x)))
                .ToLookup();
        }
    }
}
