using System.Numerics;

namespace CoTy.Objects
{
    public class Integer : Cobject<BigInteger>
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
