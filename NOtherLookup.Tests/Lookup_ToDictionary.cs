using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup;
using Tests.Utils;

namespace Tests
{
    [Subject("ILookup.ToDictionary")]
    public class When_converting_lookup_to_IDictionary
    {
        Establish context = () =>
            lookup = Lookup.Builder
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
        private static Dictionary<int, List<string>> dictionary;
    } 
    
    [Subject("ILookup.ToDictionary")]
    public class When_converting_lookup_to_IDictionary_with_comparer
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("one", new[] { "a", "b" })
                .WithKey("two", new[] { "c", "d" }).Build();

        Because of = () =>
            dictionary = lookup.ToDictionary(new StringLengthComparer());

        It should_create_dictionary_with_all_keys_from_lookup_respecting_comparer = () =>
            dictionary.Count.ShouldEqual(1);

        It should_have_valid_values_inside_with_all_elements_for_key_respecting_comparer = () =>
        {
            dictionary["one"].ShouldContainExactly("a", "b", "c", "d");
            dictionary["two"].ShouldContainExactly("a", "b", "c", "d");
        };

        private static ILookup<string, string> lookup;
        private static Dictionary<string, List<string>> dictionary;
    }

    [Subject("ILookup.ToDictionary")]
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