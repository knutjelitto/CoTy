using System;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public abstract partial class Cobject
    {
        public int? Compare(Characters chars1, Characters chars2)
        {
            return string.Compare(chars1.Value, chars2.Value, StringComparison.Ordinal);
        }

        public Characters Add(Characters chars1, Characters chars2)
        {
            return chars1.Value + chars2.Value;
        }

        public Cobject Concat(Characters chars1, Characters chars2)
        {
            return new Characters(chars1.Value + chars2.Value);
        }
    }
}
