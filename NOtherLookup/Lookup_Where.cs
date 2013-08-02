using System;
using System.Collections.Generic;
using System.Linq;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        public static ILookup<TKey, TValue> Where<TKey, TValue>(this ILookup<TKey, TValue> lookup, Func<TValue, bool> valuePredicate)
        {
            if (lookup == null)
                throw new ArgumentNullException("lookup");

            return lookup
                .Select(x => new KeyValuePair<TKey, IEnumerable<TValue>>(x.Key, x.Where(valuePredicate)))
                .ToLookup();
        }
    }
}
