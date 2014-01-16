using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TValue> Where<TKey, TValue>(
            this ILookup<TKey, TValue> lookup, Func<TValue, bool> predicate)
        {
            return lookup.OnEachKey(x => x.Where(predicate));
        }
    }
}
