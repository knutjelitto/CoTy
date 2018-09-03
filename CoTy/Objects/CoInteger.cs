using System.Numerics;

namespace CoTy.Objects
{
    public class CoInteger : CoObject<BigInteger>
    {
        public CoInteger(BigInteger value) : base(value)
        {
        }

        public CoInteger Add(CoInteger other)
        {
            return new CoInteger(Value + other.Value);
        }

        public CoString Add(CoString other)
        {
            return new CoString(Value.ToString() + other.Value);
        }

        public CoObject Add(dynamic other)
        {
            return other.CoAdd(this);
        }

        public CoInteger Sub(CoInteger other)
        {
            return new CoInteger(Value - other.Value);
        }

        public CoObject Sub(dynamic other)
        {
            return other.CoSub(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
