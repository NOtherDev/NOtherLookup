using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        public static ILookup<TKey, TValue> Concat<TKey, TValue>(this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second)
        {
            return first.ToDictionary().Concat(second.ToDictionary()).ToLookup();
        }
    }
}