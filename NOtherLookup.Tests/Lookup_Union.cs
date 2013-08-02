using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    public class When_unionizing_lookups
    {
        Establish context = () =>
            lookup = LookupBuilder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

        Because of = () =>
            concatenated = lookup.Union(LookupBuilder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build());

        It should_create_lookup_with_keys_from_both_lookups = () =>
            concatenated.Count.ShouldEqual(3);

        It should_have_unionized_IEnumerables_inside = () =>
        {
            concatenated[1].ShouldContainExactly("a", "b");
            concatenated[2].ShouldContainExactly("c", "d", "e");
            concatenated[3].ShouldContainExactly("f", "g");
        };
        
        private static ILookup<int, string> lookup, concatenated;
    }

    public class When_unionizing_null_with_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Union(LookupBuilder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build()));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }

    public class When_unionizing_lookup_with_null
    {
        Establish context = () =>
            lookup = LookupBuilder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.Union(null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }
}