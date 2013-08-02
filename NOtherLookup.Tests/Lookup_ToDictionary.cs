using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    public class When_converting_lookup_to_IDictionary
    {
        Establish context = () =>
            lookup = LookupBuilder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

        Because of = () =>
            dictionary = lookup.ToDictionary();

        It should_create_dictionary_with_all_keys_from_lookup = () =>
            dictionary.Count.ShouldEqual(2);

        It should_have_valid_values_inside = () =>
        {
            dictionary[1].ShouldContainExactly("a", "b");
            dictionary[2].ShouldContainExactly("c", "d");
        };

        private static ILookup<int, string> lookup;
        private static IDictionary<int, IEnumerable<string>> dictionary;
    }

    public class When_converting_null_to_IDictionary
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.ToDictionary());

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }
}