using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup;
using Tests.Utils;

namespace Tests
{
    [Subject("IDictionary.ToLookup")]
    public class Dictionary_ToLookup_basic
    {
        [Behaviors]
        public class When_converting_IDictionary_to_lookup
        {
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

            protected static ILookup<int, string> lookup;
        }

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

            Behaves_like<When_converting_IDictionary_to_lookup> proper_lookup;

            private static IDictionary<int, IEnumerable<string>> dictionary;
            protected static ILookup<int, string> lookup;
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

            Behaves_like<When_converting_IDictionary_to_lookup> proper_lookup;

            private static Dictionary<int, IList<string>> dictionary;
            protected static ILookup<int, string> lookup;
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

            Behaves_like<When_converting_IDictionary_to_lookup> proper_lookup;

            private static Dictionary<int, ICollection<string>> dictionary;
            protected static ILookup<int, string> lookup;
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

            Behaves_like<When_converting_IDictionary_to_lookup> proper_lookup;

            private static Dictionary<int, string[]> dictionary;
            protected static ILookup<int, string> lookup;
        }
    }

    [Subject("IDictionary.ToLookup")]
    public class Dictionary_ToLookup_null_values
    {
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
    }

    [Subject("IDictionary.ToLookup")]
    public class Dictionary_ToLookup_null_source
    {
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

    [Subject("IDictionary.ToLookup")]
    public class Dictionary_ToLookup_comparer
    {
        [Behaviors]
        public class When_converting_IDictionary_to_lookup_with_comparer
        {
            It should_create_lookup_with_keys_respecting_comparer = () =>
                lookup.Count.ShouldEqual(1);

            It should_concatenate_all_values_for_key_respecting_comparer = () =>
                lookup["one"].ShouldContainExactly("a", "b", "b", "c");

            protected static ILookup<string, string> lookup;
        }

        public class When_converting_IDictionary_of_IEnumerables_to_lookup_with_comparer
        {
            Establish context = () =>
                dictionary = new Dictionary<string, IEnumerable<string>>()
            {
                { "one", new[] { "a", "b" }},
                { "two", new[] { "b", "c" }}
            };

            Because of = () =>
                lookup = dictionary.ToLookup(new StringLengthComparer());

            Behaves_like<When_converting_IDictionary_to_lookup_with_comparer> proper_lookup;

            private static IDictionary<string, IEnumerable<string>> dictionary;
            protected static ILookup<string, string> lookup;
        }

        public class When_converting_IDictionary_of_ILists_to_lookup_with_comparer
        {
            Establish context = () =>
                dictionary = new Dictionary<string, IList<string>>()
            {
                { "one", new[] { "a", "b" }},
                { "two", new[] { "b", "c" }}
            };

            Because of = () =>
                lookup = dictionary.ToLookup(new StringLengthComparer());

            Behaves_like<When_converting_IDictionary_to_lookup_with_comparer> proper_lookup;

            private static Dictionary<string, IList<string>> dictionary;
            protected static ILookup<string, string> lookup;
        }

        public class When_converting_IDictionary_of_Lists_typed_as_ICollection_to_lookup_with_comparer
        {
            Establish context = () =>
                dictionary = new Dictionary<string, ICollection<string>>()
            {
                { "one", new[] { "a", "b" }},
                { "two", new[] { "b", "c" }}
            };

            Because of = () =>
                lookup = dictionary.ToLookup(new StringLengthComparer());

            Behaves_like<When_converting_IDictionary_to_lookup_with_comparer> proper_lookup;

            private static Dictionary<string, ICollection<string>> dictionary;
            protected static ILookup<string, string> lookup;
        }

        public class When_converting_IDictionary_of_arrays_to_lookup_with_comparer
        {
            Establish context = () =>
                dictionary = new Dictionary<string, string[]>()
            {
                { "one", new[] { "a", "b" }},
                { "two", new[] { "b", "c" }}
            };

            Because of = () =>
                lookup = dictionary.ToLookup(new StringLengthComparer());

            Behaves_like<When_converting_IDictionary_to_lookup_with_comparer> proper_lookup;

            private static Dictionary<string, string[]> dictionary;
            protected static ILookup<string, string> lookup;
        }
    }
}