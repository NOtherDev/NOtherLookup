using System.Linq;
using System.Collections.Generic;
using System;
using Machine.Specifications;

namespace Tests.Utils
{
    public static class ShouldExtensions
    {
        public static void ShouldContainExactly<T>(this IEnumerable<T> actual, params T[] expected)
        {
            if (actual.SequenceEqual(expected))
                return;
            
            var actualMissingInExpected = actual.Except(expected);
            if (actualMissingInExpected.Any())
                throw new SpecificationException("Collections expected to be equal, but expected does not contain:\n" + String.Join("\n", actualMissingInExpected));

            var expectedMissingInActual = expected.Except(actual);
            if (expectedMissingInActual.Any())
                throw new SpecificationException("Collections expected to be equal, but actual does not contain:\n" + String.Join("\n", expectedMissingInActual));

            if (actual.Count() != expected.Count())
                throw new SpecificationException(String.Format("Collections expected to be equal, but actual length ({0}) is different than expected ({1}) due to duplicate values.", actual.Count(), expected.Count()));
        }
    }
}