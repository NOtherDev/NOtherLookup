using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup;
using Tests.Utils;

namespace Tests
{
    [Subject("ILookup.Intersect")]
    public class When_intersecting_lookups
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(0, new[] { "a", "b" })
                .WithKey(1, new[] { "c", "d" })
                .WithKey(2, new[] { "e", "f", "f" }).Build();

        Because of = () =>
            intersection = lookup.Intersect(Lookup.Builder
                .WithKey(2, new[] { "f", "g", "h" })
                .WithKey(1, new[] { "a", "b" })
                .WithKey(3, new[] { "i", "j" }).Build());

        It should_create_lookup_with_keys_from_intersection = () =>
            intersection.Count.ShouldEqual(1);

        It should_have_only_intersection_of_IEnumerables_inside = () =>
            intersection[2].ShouldContainExactly("f");
        
        private static ILookup<int, string> lookup, intersection;
    }

    [Subject("ILookup.Intersect")]
    public class When_intersecting_lookups_with_key_comparer
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("one", new[] { "a", "b" })
                .WithKey("ONE", new[] { "c" }).Build();

        Because of = () =>
            intersection = lookup.Intersect(Lookup.Builder
                .WithKey("two", new[] { "b", "d" })
                .WithKey("TWO", new[] { "c" }).Build(), keyComparer: new StringLengthComparer());

        It should_create_lookup_with_keys_from_intersection_respecting_comparer = () =>
            intersection.Count.ShouldEqual(1);

        It should_have_only_intersection_of_IEnumerables_inside_respecting_comparer = () =>
            intersection["one"].ShouldContainExactly("b", "c");
        
        private static ILookup<string, string> lookup;
        private static ILookup<string, string> intersection;
    }

    [Subject("ILookup.Intersect")]
    public class When_intersecting_lookups_with_value_comparer
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(0, new[] { "one", "three" }).Build();

        Because of = () =>
            intersection = lookup.Intersect(Lookup.Builder
                .WithKey(0, new[] { "two", "four" }).Build(), valueComparer: new StringLengthComparer());

        It should_have_only_intersection_of_IEnumerables_inside_respecting_comparer = () =>
            intersection[0].ShouldContainExactly("one");

        private static ILookup<int, string> lookup, intersection;
    }

    [Subject("ILookup.Intersect")]
    public class When_intersecting_null_with_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Intersect(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build()));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Intersect")]
    public class When_intersecting_lookup_with_null
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.Intersect(null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }
}