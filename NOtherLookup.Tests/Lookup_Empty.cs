using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace NOtherLookup.Tests
{
    public class When_requesting_empty_lookup
    {
        Because of = () =>
            lookup = Lookup.Empty<int, string>();

        It should_be_empty = () =>
            lookup.ShouldBeEmpty();

        It should_be_created_once_per_constructed_type = () =>
            ReferenceEquals(lookup, Lookup.Empty<int, string>()).ShouldBeTrue();

        private static ILookup<int, string> lookup;
    }
}
