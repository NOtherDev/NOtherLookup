using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;
using NOtherLookup;
using Tests.Utils;

namespace Tests
{
    [Subject("ILookup.Zip")]
    public class When_zipping_lookups
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { 1, 2 })
                .WithKey(2, new[] { 3, 4, 4 }).Build();

        Because of = () =>
            zipped = lookup.Zip(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (x, y) => x + y);

        It should_create_lookup_with_keys_from_first_lookup_that_has_matching_keys_in_second_lookup = () =>
            zipped.Single().Key.ShouldEqual(2);

        It should_have_elements_zipped_according_to_selector_up_to_shorters_collection_length = () => 
            zipped[2].ShouldContainExactly("3e", "4d");
        
        private static ILookup<int, int> lookup;
        private static ILookup<int, string> zipped;
    }

    [Subject("ILookup.Zip")]
    public class When_zipping_lookups_with_key_comparer
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("one", new[] { 1, 2 })
                .WithKey("ONE", new[] { 3 }).Build();

        Because of = () =>
            zipped = lookup.Zip(Lookup.Builder
                .WithKey("two", new[] { "a", "b" })
                .WithKey("TWO", new[] { "c", "d" }).Build(), (x, y) => x + y, new StringLengthComparer());

        It should_create_lookup_with_keys_from_first_lookup_that_has_matching_keys_in_second_lookup_respecting_comparer = () =>
            zipped.Single().Key.ShouldEqual("one");

        It should_have_elements_zipped_according_to_selector_up_to_shorters_collection_length_respecting_comparer = () => 
            zipped["two"].ShouldContainExactly("1a", "2b", "3c");
        
        private static ILookup<string, int> lookup;
        private static ILookup<string, string> zipped;
    }

    [Subject("ILookup.Zip")]
    public class When_zipping_null_with_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.Zip(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (x, y) => x + y));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Zip")]
    public class When_zipping_lookup_with_null
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { "a", "b" })
                .WithKey(2, new[] { "c", "d" }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.Zip<int, string, int, string>(null, (x, y) => x + y));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, string> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.Zip")]
    public class When_zipping_lookups_with_null_result_selector
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey(1, new[] { 1, 2 })
                .WithKey(2, new[] { 3, 4, 4 }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.Zip(Lookup.Builder
                .WithKey(2, new[] { "e", "d" })
                .WithKey(3, new[] { "f", "g" }).Build(), (Func<int, string, string>) null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<int, int> lookup;
        private static Exception exception;
    }
}