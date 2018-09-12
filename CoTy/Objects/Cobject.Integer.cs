// ReSharper disable UnusedMember.Global
using System;

namespace CoTy.Objects
{
    public partial class Cobject : ICompare<Integer>
    {
        public int? Compare(Integer value1, Integer value2)
        {
            return value1.Value.CompareTo(value2.Value);
        }

        public Integer Plus(Integer value1, Integer value2)
        {
            return Integer.From(value1.Value + value2.Value);
        }

        public Integer Minus(Integer value1, Integer value2)
        {
            return Integer.From(value1.Value - value2.Value);
        }

        public Integer Star(Integer value1, Integer value2)
        {
            return Integer.From(value1.Value * value2.Value);
        }

        public Integer Slash(Integer value1, Integer value2)
        {
            return Integer.From(value1.Value / value2.Value);
        }

        public Integer Succ(Integer value)
        {
            return Integer.From(value.Value + 1);
        }

        public Integer Pred(Integer value)
        {
            return Integer.From(value.Value - 1);
        }
    }
}
