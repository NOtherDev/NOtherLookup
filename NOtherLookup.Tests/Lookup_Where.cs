using System;
using System.Collections.Generic;
using System.Linq;
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
            filtered = lookup.Where(x => x > 3);

        It should_filter_out_values_according_to_predicate = () =>
        {
            filtered.Count.ShouldEqual(1);
            filtered["b"].ShouldContainExactly(4);
        };

        private static ILookup<string, int> lookup, filtered;
    }
    
    public class When_filtering_lookup_by_values_and_indexes
    {
        Establish context = () =>
            lookup = LookupBuilder
                .WithKey("a", new[] { 3, 1 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            filtered = lookup.Where((x, i) => x > 3 || i > 0);

        It should_filter_out_values_according_to_predicate = () =>
        {
            filtered.Count.ShouldEqual(2);
            filtered["a"].ShouldContainExactly(1);
            filtered["b"].ShouldContainExactly(4);
        };

        private static ILookup<string, int> lookup, filtered;
    }

    public class When_filtering_null_lookup_by_values
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
    
    public class When_filtering_null_lookup_by_values_and_indexes
    {
        Establish context = () =>
            lookup = null;

        Because of = () => 
            exception = Catch.Exception(() => lookup.Where((x, i) => x > 3 || i > 0));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }

    public class When_filtering_lookup_by_null_predicate
    {
        Establish context = () =>
            lookup = LookupBuilder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            exception = Catch.Exception(() => lookup.Where((Func<int, bool>)null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }    
    
    public class When_filtering_lookup_by_null_predicate_with_index
    {
        Establish context = () =>
            lookup = LookupBuilder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.Where((Func<int, int, bool>)null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }
}
