using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    public class When_converting_IDictionary_of_IEnumerables_to_lookup
    {
        Establish context = () =>
            dictionary = new Dictionary<int, IEnumerable<string>>()
            {
                { 1, new[] { "a", "b" }},
                { 2, new[] { "c", "d" }},
                { 3, new string[0] }
            };

        Because of = () =>
            lookup = dictionary.ToLookup();

        It should_create_lookup_with_keys_only_for_not_empty_IEnumerables = () =>
            lookup.Count.ShouldEqual(2);

        It should_have_valid_IEnumerables_inside = () =>
        {
            lookup[1].ShouldContainExactly("a", "b");
            lookup[2].ShouldContainExactly("c", "d");
        };

        It should_return_empty_collections_for_unknown_keys = () =>
        {
            lookup[3].ShouldBeEmpty();
            lookup[4].ShouldBeEmpty();
        };

        private static IDictionary<int, IEnumerable<string>> dictionary;
        private static ILookup<int, string> lookup;
    }

    public class When_converting_IDictionary_of_IEnumerables_with_null_to_lookup
    {
        Establish context = () =>
            dictionary = new Dictionary<int, IEnumerable<string>>()
            {
                { 0, null }
            };

        Because of = () =>
            lookup = dictionary.ToLookup();

        It should_create_empty_lookup = () =>
            lookup.ShouldBeEmpty();

        private static IDictionary<int, IEnumerable<string>> dictionary;
        private static ILookup<int, string> lookup;
    }
    
    public class When_converting_null_of_IEnumerables_to_lookup
    {
        Establish context = () =>
            dictionary = null;

        Because of = () =>
            exception = Catch.Exception(() => dictionary.ToLookup());

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static IDictionary<int, IEnumerable<string>> dictionary;
        private static Exception exception;
    }
    
    public class When_converting_IDictionary_of_ILists_to_lookup
    {
        Establish context = () =>
            dictionary = new Dictionary<int, IList<string>>()
            {
                { 1, new[] { "a", "b" }},
                { 2, new[] { "c", "d" }},
                { 3, new string[0] }
            };

        Because of = () =>
            lookup = dictionary.ToLookup();

        It should_create_lookup_with_keys_only_for_not_empty_IEnumerables = () =>
            lookup.Count.ShouldEqual(2);

        It should_have_valid_IEnumerables_inside = () =>
        {
            lookup[1].ShouldContainExactly("a", "b");
            lookup[2].ShouldContainExactly("c", "d");
        };

        It should_return_empty_collections_for_unknown_keys = () =>
        {
            lookup[3].ShouldBeEmpty();
            lookup[4].ShouldBeEmpty();
        };

        private static Dictionary<int, IList<string>> dictionary;
        private static ILookup<int, string> lookup;
    }

    public class When_converting_IDictionary_of_ILists_with_null_to_lookup
    {
        Establish context = () =>
            dictionary = new Dictionary<int, IList<string>>()
            {
                { 0, null }
            };

        Because of = () =>
            lookup = dictionary.ToLookup();

        It should_create_empty_lookup = () =>
            lookup.ShouldBeEmpty();

        private static Dictionary<int, IList<string>> dictionary;
        private static ILookup<int, string> lookup;
    }
    
    public class When_converting_null_of_ILists_to_lookup
    {
        Establish context = () =>
            dictionary = null;

        Because of = () =>
            exception = Catch.Exception(() => dictionary.ToLookup());

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static IDictionary<int, IList<string>> dictionary;
        private static Exception exception;
    }

    public class When_converting_IDictionary_of_Lists_typed_as_ICollection_to_lookup
    {
        Establish context = () =>
            dictionary = new Dictionary<int, ICollection<string>>()
            {
                { 1, new List<string>() { "a", "b" }},
                { 2, new List<string>() { "c", "d" }},
                { 3, new List<string>() }
            };

        Because of = () =>
            lookup = dictionary.ToLookup();

        It should_create_lookup_with_keys_only_for_not_empty_IEnumerables = () =>
            lookup.Count.ShouldEqual(2);

        It should_have_valid_IEnumerables_inside = () =>
        {
            lookup[1].ShouldContainExactly("a", "b");
            lookup[2].ShouldContainExactly("c", "d");
        };

        It should_return_empty_collections_for_unknown_keys = () =>
        {
            lookup[3].ShouldBeEmpty();
            lookup[4].ShouldBeEmpty();
        };

        private static Dictionary<int, ICollection<string>> dictionary;
        private static ILookup<int, string> lookup;
    }

    public class When_converting_IDictionary_of_ICollection_with_null_to_lookup
    {
        Establish context = () =>
            dictionary = new Dictionary<int, ICollection<string>>()
            {
                { 0, null }
            };

        Because of = () =>
            lookup = dictionary.ToLookup();

        It should_create_empty_lookup = () =>
            lookup.ShouldBeEmpty();

        private static Dictionary<int, ICollection<string>> dictionary;
        private static ILookup<int, string> lookup;
    }

    public class When_converting_null_of_ICollections_to_lookup
    {
        Establish context = () =>
            dictionary = null;

        Because of = () =>
            exception = Catch.Exception(() => dictionary.ToLookup());

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static IDictionary<int, ICollection<string>> dictionary;
        private static Exception exception;
    }

    public class When_converting_IDictionary_of_arrays_to_lookup
    {
        Establish context = () =>
            dictionary = new Dictionary<int, string[]>()
            {
                { 1, new[] { "a", "b" }},
                { 2, new[] { "c", "d" }},
                { 3, new string[0] }
            };

        Because of = () =>
            lookup = dictionary.ToLookup();

        It should_create_lookup_with_keys_only_for_not_empty_IEnumerables = () =>
            lookup.Count.ShouldEqual(2);

        It should_have_valid_IEnumerables_inside = () =>
        {
            lookup[1].ShouldContainExactly("a", "b");
            lookup[2].ShouldContainExactly("c", "d");
        };

        It should_return_empty_collections_for_unknown_keys = () =>
        {
            lookup[3].ShouldBeEmpty();
            lookup[4].ShouldBeEmpty();
        };

        private static Dictionary<int, string[]> dictionary;
        private static ILookup<int, string> lookup;
    }

    public class When_converting_IDictionary_of_arrays_with_null_to_lookup
    {
        Establish context = () =>
            dictionary = new Dictionary<int, string[]>()
            {
                { 0, null }
            };

        Because of = () =>
            lookup = dictionary.ToLookup();

        It should_create_empty_lookup = () =>
            lookup.ShouldBeEmpty();

        private static Dictionary<int, string[]> dictionary;
        private static ILookup<int, string> lookup;
    }

    public class When_converting_null_of_arrays_to_lookup
    {
        Establish context = () =>
            dictionary = null;

        Because of = () =>
            exception = Catch.Exception(() => dictionary.ToLookup());

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static IDictionary<int, string[]> dictionary;
        private static Exception exception;
    }
}