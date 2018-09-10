// ReSharper disable UnusedMember.Global

using System;
using CoTy.Errors;

namespace CoTy.Objects
{
    public abstract partial class Cobject
    {
        public Bool NotEquals(dynamic other)
        {
            return ((Bool)this.Equals(other)).Not();
        }

        public Bool Less(dynamic other)
        {
            throw new TypeMismatchException($"can't ordered compare `{this}´ with `{other}´");
        }

        public Bool NotLess(dynamic other)
        {
            return ((dynamic)this).Less(other).Not();
        }

        public Bool Greater(dynamic other)
        {
            return NotEquals(other) && NotLess(other);
        }

        public Bool LessOrEquals(dynamic other)
        {
            return ((dynamic)this).Less(other) || ((dynamic)this).Equals(other);
        }

        public Bool GreaterOrEquals(dynamic other)
        {
            return ((dynamic)this).Greater(other) || ((dynamic)this).Equals(other);
        }

        public void Print()
        {
            Console.Write($"{this}");
        }

        public void Println()
        {
            Print();
            Console.WriteLine();
        }
    }
}
