using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    public class When_filtering_lookup_by_values
    {
        Establish context = () =>
            lookup = LookupBuilder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            filtered = lookup.Where(x => x <= 2);

        private static ILookup<string, int> lookup, filtered;
    }

    public static class X
    {
        public static ILookup<K,V> Where<K,V>(this ILookup<K,V> t, Func<V, bool> p)
        {
            return t;
        } 
    }
}
