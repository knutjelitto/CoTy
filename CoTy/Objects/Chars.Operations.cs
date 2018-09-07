// ReSharper disable UnusedMember.Global

using System;

namespace CoTy.Objects
{
    public partial class Chars : IComparable<Chars>, IOrdered<Chars>
    {
        // equality

        public Bool Equals(Chars other)
        {
            return Bool.From(Value == other.Value);
        }

        // ordering

        public Bool Less(Chars other)
        {
            return Bool.From(string.Compare(Value, other.Value, StringComparison.Ordinal) < 0);
        }

        public Chars Concatenate(Chars other)
        {
            return new Chars(Value + other.Value);
        }

        public Cobject Concatenate(dynamic other)
        {
            return other.CoConcatenate(this);
        }
    }
}
