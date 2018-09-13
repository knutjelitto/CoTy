using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoTy.Objects
{
    public static class EnumerableEx
    {
        public static IEnumerable<T> Enumerate<T>(params T[] arguments)
        {
            return arguments;
        }
    }
}
