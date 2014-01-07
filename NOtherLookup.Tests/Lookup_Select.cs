using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    [Subject("ILookup.Select")]
    public class When_filtering_lookup_by_values
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            filtered = lookup.Select(x => x.Where(e => e > 3));

        It should_filter_out_values_according_to_predicate = () =>
        {
            filtered.Count.ShouldEqual(1);
            filtered["b"].ShouldContainExactly(4);
        };

        private static ILookup<string, int> lookup, filtered;
    }

    [Subject("ILookup.Select")]
    public class When_filtering_null_lookup_by_values
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Select(x => x.Where(e => e > 3)));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Select")]
    public class When_traversing_lookup_with_null_function
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            exception = Catch.Exception(() => lookup.Select((Func<IEnumerable<int>, IEnumerable<bool>>)null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }    
}
