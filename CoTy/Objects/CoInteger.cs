using System.Numerics;

namespace CoTy.Objects
{
    public class CoInteger : CoSelfish<BigInteger>
    {
        public CoInteger(BigInteger value) : base(value)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
