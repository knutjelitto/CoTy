// ReSharper disable UnusedMember.Global

using System;

namespace CoTy.Objects
{
    public partial class Characters
    {
        // ordering

        public Bool Less(Characters other)
        {
            return Bool.From(string.Compare(Value, other.Value, StringComparison.Ordinal) < 0);
        }

        public Characters Concatenate(Characters other)
        {
            return new Characters(Value + other.Value);
        }

        public Cobject Concatenate(dynamic other)
        {
            return other.CoConcatenate(this);
        }
    }
}
