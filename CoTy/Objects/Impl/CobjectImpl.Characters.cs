using System;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects.Impl
{
    public partial class CobjectImpl
    {
        public int Compare(Characters chars1, Characters chars2)
        {
            return string.Compare(chars1.Value, chars2.Value, StringComparison.Ordinal);
        }

        public Characters Plus(Characters chars1, Characters chars2)
        {
            return chars1.Value + chars2.Value;
        }
    }
}
