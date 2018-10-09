using System;
using System.Text;

namespace CoTy.Objects
{
    public sealed class Chars : Cobject<string>, IComparable<Chars>
    {
        private Chars(string value) : base(value)
        {
        }

        public static Chars From(string value)
        {
            return new Chars(value);
        }

        public int CompareTo(Chars other)
        {
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Chars other && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static Chars operator +(Chars value1, Chars value2)
        {
            return From(value1.Value + value2.Value);
        }

        public static Chars operator *(Chars value1, Integer value2)
        {
            return value1.MultiplyBy(value2);
        }

        public static Chars operator *(Integer value1, Chars value2)
        {
            return value2.MultiplyBy(value1);
        }

        public override string ToString()
        {
            return Value;
        }

        private Chars MultiplyBy(Integer count)
        {
            var builder = new StringBuilder();
            while (count > 0)
            {
                builder.Append(Value);
                count--;
            }

            return From(builder.ToString());
        }
    }
}
