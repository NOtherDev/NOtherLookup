using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NOtherLookup.Tests.Utils;

namespace NOtherLookup.Tests
{
    public class When_building_lookup
    {
        Because of = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(1, new[] { "e", "f" })
                .WithKey(2, new[] { "c", "d", "d" })
                .WithKey(3, null)
                .Build();

        It should_be_enumerable_and_contain_two_keys = () =>
            lookup.Select(x => x.Key).ShouldContainExactly(1, 2);

        It should_keep_the_latest_definition_for_key = () =>
            lookup[1].ShouldContainExactly("e", "f");

        It should_keep_provided_collections_unchanged = () =>
            lookup[2].ShouldContainExactly("c", "d", "d");

        It should_ignore_null_collections_and_treat_as_undefined = () =>
            lookup[3].ShouldBeEmpty();

        It should_return_empty_collections_for_unknown_keys = () =>
            lookup[4].ShouldBeEmpty();

        It should_provide_proper_count = () =>
            lookup.Count.ShouldEqual(2);

        It should_properly_indicate_key_existence = () =>
        {
            lookup.Contains(1).ShouldBeTrue();
            lookup.Contains(2).ShouldBeTrue();
            lookup.Contains(3).ShouldBeFalse();
            lookup.Contains(4).ShouldBeFalse();
        };

        private static ILookup<int, string> lookup;
    }
    
    public class When_building_lookup_with_null_key
    {
        Because of = () =>
            lookup = Lookup.Builder
                .WithKey("not null", new[] { "a", "b" })
                .WithKey(null, new[] { "e", "f" })
                .Build();

        It should_be_enumerable_and_contain_two_keys = () =>
            lookup.Select(x => x.Key).ShouldContainExactly("not null", null);

        It should_properly_return_values_for_null_key = () =>
            lookup[null].ShouldContainExactly("e", "f");

        It should_provide_proper_count = () =>
            lookup.Count.ShouldEqual(2);

        It should_properly_indicate_null_key_existence = () =>
            lookup.Contains(null).ShouldBeTrue();

        private static ILookup<string, string> lookup;
    }

    public class When_modifying_source_collection_after_lookup_was_built
    {
        Establish context = () =>
        {
            source = new List<string>() { "a", "b" };

            lookup = Lookup.Builder
                .WithKey("key", source)
                .Build();
        };

        Because of = () =>
            source.Add("c");

        It should_not_be_dependent_upon_source_collection_modifications = () =>
            lookup["key"].ShouldContainExactly("a", "b");

        private static ILookup<string, string> lookup;
        private static List<string> source;
    }

    public class When_building_lookup_with_custom_comparer
    {
        Because of = () =>
            lookup = Lookup.Builder
                .WithComparer(new StringLengthComparer())
                .WithKey("one", new[] { "a", "b" })
                .WithKey("two", new[] { "e", "f" })
                .WithKey("three", new[] { "c", "d", "d" })
                .Build();

        private class StringLengthComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null || y == null)
                    return false;
                return x.Length == y.Length;
            }

            public int GetHashCode(string obj)
            {
                return obj.Length;
            }
        }

        It should_be_enumerable_and_contain_two_keys_respecting_comparer = () =>
            lookup.Select(x => x.Key).ShouldContainExactly("one", "three");

        It should_keep_the_latest_definition_for_key_respecting_comparer = () =>
            lookup["one"].ShouldContainExactly("e", "f");

        It should_use_comparer_to_look_for_values = () =>
        {
            lookup["two"].ShouldBeTheSameAs(lookup["one"]);
            lookup["abc"].ShouldBeTheSameAs(lookup["one"]);
        };

        It should_provide_proper_count_respecting_comparer = () =>
            lookup.Count.ShouldEqual(2);

        It should_properly_indicate_key_existence_respecting_comparer = () =>
        {
            lookup.Contains("one").ShouldBeTrue();
            lookup.Contains("two").ShouldBeTrue();
            lookup.Contains("abc").ShouldBeTrue();
            lookup.Contains("four").ShouldBeFalse();
        };

        private static ILookup<string, string> lookup;
    }

    public class When_building_lookup_with_custom_comparer_that_does_magic_with_nulls
    {
        Because of = () =>
            lookup = Lookup.Builder
                .WithComparer(new StringLengthComparerWithStringifiedNull())
                .WithKey("", new[] { "a", "b" })
                .WithKey(null, new[] { "c", "d" })
                .Build();

        private class StringLengthComparerWithStringifiedNull : IEqualityComparer<string>
        {
            private string StringifyNullIfNeeded(string obj)
            {
                return obj ?? String.Empty;
            }

            public bool Equals(string x, string y)
            {
                return StringifyNullIfNeeded(x).Length == StringifyNullIfNeeded(y).Length;
            }

            public int GetHashCode(string obj)
            {
                return StringifyNullIfNeeded(obj).Length;
            }
        }

        It should_be_enumerable_and_contain_one_key_respecting_comparer = () =>
            lookup.Select(x => x.Key).ShouldContainExactly("");

        It should_keep_the_latest_definition_for_key_respecting_comparer = () =>
            lookup[""].ShouldContainExactly("c", "d");

        It should_use_comparer_to_look_for_values = () =>
            lookup[null].ShouldBeTheSameAs(lookup[""]);

        It should_provide_proper_count_respecting_comparer = () =>
            lookup.Count.ShouldEqual(1);

        It should_properly_indicate_key_existence_respecting_comparer = () =>
        {
            lookup.Contains("").ShouldBeTrue();
            lookup.Contains(null).ShouldBeTrue();
        };

        private static ILookup<string, string> lookup;
    }
}
