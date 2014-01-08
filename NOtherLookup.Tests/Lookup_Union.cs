using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    [Subject("ILookup.Union")]
    public class When_unionizing_lookups
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

        Because of = () =>
            concatenated = lookup.Union(Lookup.Builder
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
    
    [Subject("ILookup.Union")]
    public class When_unionizing_lookups_with_comparer
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("one", new[] { "a", "b" })
                .WithKey("ONE", new[] { "c" }).Build();

        Because of = () =>
            concatenated = lookup.Union(Lookup.Builder
                .WithKey("two", new[] { "c", "d" })
                .WithKey("TWO", new[] { "e" }).Build(), new StringLengthComparer());

        It should_create_lookup_with_keys_from_both_lookups_respecting_comparer = () =>
            concatenated.Count.ShouldEqual(1);

        It should_have_unionized_IEnumerables_inside_respecting_comparer = () =>
            concatenated["two"].ShouldContainExactly("a", "b", "c", "d", "e");
        
        private static ILookup<string, string> lookup, concatenated;
    }

    [Subject("ILookup.Union")]
    public class When_unionizing_null_with_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Union(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build()));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Union")]
    public class When_unionizing_lookup_with_null
    {
        Establish context = () =>
            lookup = Lookup.Builder
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