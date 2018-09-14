using System;
using System.Numerics;

namespace CoTy.Objects
{
    public class Integer : Cobject<BigInteger>, IComparable<Integer>
    {
        public static readonly Integer Zero = new Integer(0);
        public static Integer One = new Integer(1);

        private Integer(BigInteger value) : base(value)
        {
        }

        public static Integer From(BigInteger value)
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

        public int CompareTo(Integer other)
        {
            return Value.CompareTo(other.Value);
        }

        public static explicit operator int(Integer value)
        {
            return (int) value.Value;
        }

        public static Integer operator +(Integer x, Integer y)
        {
            return From(x.Value + y.Value);
        }

        public static Integer operator -(Integer x, Integer y)
        {
            return From(x.Value - y.Value);
        }

        public static Integer operator *(Integer x, Integer y)
        {
            return From(x.Value * y.Value);
        }

        public static Integer operator /(Integer x, Integer y)
        {
            return From(x.Value / y.Value);
        }

        public static Integer operator %(Integer x, Integer y)
        {
            return From(x.Value % y.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Integer other && Equals(Value, other.Value);
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
