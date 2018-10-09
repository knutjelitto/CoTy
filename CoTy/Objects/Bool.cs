using System;

namespace CoTy.Objects
{
    public sealed class Bool : Cobject<bool>, IComparable<Bool>
    {
        public static Bool False = new Bool(false);
        public static Bool True = new Bool(true);

        private Bool(bool value) : base(value)
        {
        }

        public static Bool From(bool value)
        {
            return value ? True : False;
        }

        public int CompareTo(Bool other)
        {
            return Value ? (other.Value ? 0 : 1) : (other.Value ? -1 : 0);
        }

        public override bool Equals(object obj)
        {
            return obj is Bool other && ReferenceEquals(this, other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static Bool operator &(Bool value1, Bool value2)
        {
            return From(value1.Value & value2.Value);
        }

        public static Bool operator |(Bool value1, Bool value2)
        {
            return From(value1.Value | value2.Value);
        }

        public static Bool operator ^(Bool value1, Bool value2)
        {
            return From(value1.Value ^ value2.Value);
        }

        public static Bool operator !(Bool value)
        {
            return From(!value.Value);
        }

        public override string ToString()
        {
            return Value ? "true" : "false";
        }
    }
}
