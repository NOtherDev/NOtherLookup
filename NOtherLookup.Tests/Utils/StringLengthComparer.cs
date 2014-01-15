using System.Linq;
using System.Collections.Generic;
using System;

namespace Tests.Utils
{
    internal class StringLengthComparer : IEqualityComparer<string>
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
}