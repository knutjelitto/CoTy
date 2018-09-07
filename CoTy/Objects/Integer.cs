﻿using System.Numerics;

namespace CoTy.Objects
{
    public partial class Integer : Cobject<BigInteger>, IComparable<Integer>, IOrdered<Integer>
    {
        private Integer(BigInteger value) : base(value)
        {
        }

        public static Integer From(int value)
        {
            return new Integer(value);
        }

        public static bool TryFrom(string str, out Integer value)
        {
            if (BigInteger.TryParse(str, out var parsed))
            {
                value = new Integer(parsed);
                return true;
            }
            value = null;
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is Integer other && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}