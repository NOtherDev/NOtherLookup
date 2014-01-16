using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TResult> OnEachKey<TKey, TValue, TResult>(
            this ILookup<TKey, TValue> lookup, Func<IGrouping<TKey, TValue>, IEnumerable<TResult>> transformer)
        {
            if (transformer == null)
                throw new ArgumentNullException("transformer");

            return lookup.Select(x => Pair.Of(x.Key, transformer(x))).ToLookup();
        }
    }
}
