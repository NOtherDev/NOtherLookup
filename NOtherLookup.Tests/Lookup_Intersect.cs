using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    public class When_intersecting_lookups
    {
        Establish context = () =>
            lookup = LookupBuilder
                .WithKey(0, new[] { "a", "b" })
                .WithKey(1, new[] { "c", "d" })
                .WithKey(2, new[] { "e", "f", "f" }).Build();

        Because of = () =>
            intersection = lookup.Intersect(LookupBuilder
                .WithKey(2, new[] { "f", "g", "h" })
                .WithKey(1, new[] { "a", "b" })
                .WithKey(3, new[] { "i", "j" }).Build());

        It should_create_lookup_with_keys_from_intersection = () =>
            intersection.Count.ShouldEqual(1);

        It should_have_only_intersection_of_IEnumerables_inside = () =>
            intersection[2].ShouldContainExactly("f");
        
        private static ILookup<int, string> lookup, intersection;
    }
    
    public class When_intersecting_null_with_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Intersect(LookupBuilder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build()));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }
    
    public class When_intersecting_lookup_with_null
    {
        Establish context = () =>
            lookup = LookupBuilder
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