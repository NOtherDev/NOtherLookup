﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NOtherLookup
{
    public static partial class GroupingExtensions
    {
        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings.Select(x => new KeyValuePair<TKey, IEnumerable<TValue>>(x.Key, x)).ToLookup();
        }
    }
}