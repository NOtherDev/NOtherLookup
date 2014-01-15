﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TResult> Select<TKey, TValue, TResult>(
            this ILookup<TKey, TValue> lookup, Func<TValue, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            return lookup.Select(x => Pair.Of(x.Key, x.Select(selector))).ToLookup();
        }
    }
}
