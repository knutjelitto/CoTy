using System.Numerics;

namespace CoTy.Objects
{
    public partial class Integer : Cobject<BigInteger, Integer>, IOrdered<Integer>
    {
        public static Integer Zero = new Integer(BigInteger.Zero);
        public static Integer One = new Integer(BigInteger.One);

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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
