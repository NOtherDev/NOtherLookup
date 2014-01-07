using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    [Subject("IEnumerable<IGrouping>.ToLookup")]
    public class When_converting_IEnumerable_of_IGrouping_to_lookup
    {
        Establish context = () =>
            grouping = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () =>
            lookup = grouping.ToLookup();

        It should_create_lookup_with_all_keys = () =>
            lookup.Count.ShouldEqual(2);

        It should_have_valid_IEnumerables_inside = () =>
        {
            lookup["a"].ShouldContainExactly(1, 3);
            lookup["b"].ShouldContainExactly(2, 4);
        };

        private static IEnumerable<IGrouping<string, int>> grouping;
        private static ILookup<string, int> lookup;
    }
    
    [Subject("IEnumerable<IGrouping>.ToLookup")]
    public class When_converting_IEnumerable_of_IGrouping_to_lookup_with_comparer
    {
        Establish context = () =>
            grouping = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () =>
            lookup = grouping.ToLookup(new StringLengthComparer());

        It should_create_lookup_with_all_keys_respecting_comparer = () =>
            lookup.Count.ShouldEqual(1);

        It should_have_valid_IEnumerables_inside_with_all_elements_for_key_respecting_comparer = () =>
        {
            lookup["a"].ShouldContainExactly(1, 3, 2, 4);
            lookup["b"].ShouldContainExactly(1, 3, 2, 4);
        };

        private static IEnumerable<IGrouping<string, int>> grouping;
        private static ILookup<string, int> lookup;
    }
    
    [Subject("IEnumerable<IGrouping>.ToLookup")]
    public class When_converting_null_IEnumerable_of_IGrouping_to_lookup
    {
        Establish context = () =>
            grouping = null;

        Because of = () =>
            exception = Catch.Exception(() => grouping.ToLookup());

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static IEnumerable<IGrouping<string, int>> grouping;
        private static Exception exception;
    }
}