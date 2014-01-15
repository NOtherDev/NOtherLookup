using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NOtherLookup;
using Tests.Utils;

namespace Tests
{
    [Subject("ILookup.Where")]
    public class When_filtering_lookup
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            filtered = lookup.Where(x => x > 3);

        It should_filter_values_according_to_predicate = () =>
        {
            filtered.Count.ShouldEqual(1);
            filtered["b"].ShouldContainExactly(4);
        };

        private static ILookup<string, int> lookup, filtered;
    }

    [Subject("ILookup.Where")]
    public class When_filtering_lookup_using_query_syntax
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            filtered = from x in lookup 
                       where x > 3
                       select x;

        It should_filter_values_according_to_predicate = () =>
        {
            filtered.Count.ShouldEqual(1);
            filtered["b"].ShouldContainExactly(4);
        };

        private static ILookup<string, int> lookup, filtered;
    }

    [Subject("ILookup.Where")]
    public class When_filtering_null_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Where(x => x > 3));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Where")]
    public class When_filtering_lookup_with_null_predicate
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            exception = Catch.Exception(() => lookup.Where(null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }    
}
