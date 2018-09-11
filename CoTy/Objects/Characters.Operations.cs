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

        public Cobject Concat(Characters chars1, Characters chars2)
        {
            return new Characters(chars1.Value + chars2.Value);
        }
    }
}
