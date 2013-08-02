using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOtherLookup
{
    public static partial class GroupingExtensions
    {
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings.Select(x => new KeyValuePair<TKey, IEnumerable<TValue>>(x.Key, x)).ToLookup();
        }
    }
}
