// ReSharper disable UnusedMember.Global

using Microsoft.Win32.SafeHandles;

namespace CoTy.Objects
{
    public partial class CoInteger
    {
        public CoInteger Add(CoInteger other)
        {
            return new CoInteger(Value + other.Value);
        }

        public CoString Add(CoString other)
        {
            return new CoString(Value.ToString() + other.Value);
        }

        public CoTuple Add(dynamic other)
        {
            return other.CoAdd(this);
        }

        public CoInteger Sub(CoInteger other)
        {
            return new CoInteger(Value - other.Value);
        }

        public CoTuple Sub(dynamic other)
        {
            return other.CoSub(this);
        }

        public CoInteger Mul(CoInteger other)
        {
            return new CoInteger(Value * other.Value);
        }

        public CoTuple Mul(dynamic other)
        {
            return other.CoMul(this);
        }

        public CoInteger Div(CoInteger other)
        {
            return new CoInteger(Value / other.Value);
        }

        public CoTuple Div(dynamic other)
        {
            return other.CoDiv(this);
        }

        public CoBoolean EQ(CoInteger other)
        {
            return Value == other.Value;
        }

        public CoBoolean NE(CoInteger other)
        {
            return Value != other.Value;
        }

        public CoBoolean LT(CoInteger other)
        {
            return Value < other.Value;
        }

        public CoBoolean LE(CoInteger other)
        {
            return Value <= other.Value;
        }

        public CoBoolean GT(CoInteger other)
        {
            return Value > other.Value;
        }

        public CoBoolean GE(CoInteger other)
        {
            return Value >= other.Value;
        }

        public CoInteger Succ()
        {
            return new CoInteger(Value + 1);
        }

        public CoInteger Pred()
        {
            return new CoInteger(Value - 1);
        }
    }
}
