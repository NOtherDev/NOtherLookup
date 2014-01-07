using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    [Subject("ILookup.Except")]
    public class When_creating_lookups_difference
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(0, new[] { "a", "b", "a" })
                .WithKey(1, new[] { "c", "d" })
                .WithKey(2, new[] { "e", "f", "f" }).Build();

        Because of = () =>
            difference = lookup.Except(Lookup.Builder
                .WithKey(2, new[] { "f", "g", "e" })
                .WithKey(1, new[] { "c", "b" })
                .WithKey(3, new[] { "i", "j" }).Build());

        It should_create_lookup_with_keys_from_difference = () =>
            difference.Count.ShouldEqual(2);

        It should_have_only_difference_of_IEnumerables_inside = () =>
        {
            difference[0].ShouldContainExactly("a", "b");
            difference[1].ShouldContainExactly("d");
        };
        
        private static ILookup<int, string> lookup, difference;
    }

    [Subject("ILookup.Except")]
    public class When_creating_difference_of_null_and_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Except(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build()));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Except")]
    public class When_creating_difference_of_lookup_and_null
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.Except(null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }
}