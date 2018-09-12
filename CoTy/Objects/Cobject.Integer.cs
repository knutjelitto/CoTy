// ReSharper disable UnusedMember.Global
using System;

namespace CoTy.Objects
{
    public partial class Cobject
    {
        public int? Compare(Integer value1, Integer value2)
        {
            return value1.Value.CompareTo(value2.Value);
        }
    }
}
