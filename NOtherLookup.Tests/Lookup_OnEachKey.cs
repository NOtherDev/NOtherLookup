using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NOtherLookup;
using Tests.Utils;

namespace Tests
{
    [Subject("ILookup.OnEachKey")]
    public class When_applying_function_on_each_lookup_key
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () => 
            transformed = lookup.OnEachKey(x => x.Select(e => x.Key + e).OrderByDescending(e => e));

        It should_transform_values_using_functions_provided = () =>
        {
            transformed.Count.ShouldEqual(2);
            transformed["a"].ShouldContainExactly("a3", "a1");
            transformed["b"].ShouldContainExactly("b4", "b2");
        };

        private static ILookup<string, int> lookup;
        private static ILookup<string, string> transformed;
    }

    [Subject("ILookup.OnEachKey")]
    public class When_applying_function_on_null_lookup
    {
        Establish context = () =>
            lookup = null;

        Because of = () =>
            exception = Catch.Exception(() => lookup.OnEachKey(x => x.Select(e => x.Key + e).OrderByDescending(e => e)));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }

    [Subject("ILookup.OnEachKey")]
    public class When_applying_null_function_on_lookup
    {
        Establish context = () =>
            lookup = Lookup.Builder
                .WithKey("a", new[] { 1, 3 })
                .WithKey("b", new[] { 2, 4 }).Build();

        Because of = () =>
            exception = Catch.Exception(() => lookup.OnEachKey<string, int, int>(null));

        It should_throw_ArgumentNullException = () =>
            exception.ShouldBeOfType<ArgumentNullException>();

        private static ILookup<string, int> lookup;
        private static Exception exception;
    }    
}
