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

        protected static ILookup<int, string> lookup;
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

        It should_properly_indicate_nukk_key_existence = () =>
            lookup.Contains(null).ShouldBeTrue();

        protected static ILookup<string, string> lookup;
    }
}
