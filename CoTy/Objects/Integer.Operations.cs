// ReSharper disable UnusedMember.Global

namespace CoTy.Objects
{
    public partial class Integer
    {
        public Integer Add(Integer other)
        {
            return new Integer(Value + other.Value);
        }

        public Cobject Add(dynamic other)
        {
            return other.CoAdd(this);
        }

        public Integer Sub(Integer other)
        {
            return new Integer(Value - other.Value);
        }

        public Cobject Sub(dynamic other)
        {
            return other.CoSub(this);
        }

        public Integer Mul(Integer other)
        {
            return new Integer(Value * other.Value);
        }

        public Cobject Mul(dynamic other)
        {
            return other.CoMul(this);
        }

        public Integer Div(Integer other)
        {
            return new Integer(Value / other.Value);
        }

        public Cobject Div(dynamic other)
        {
            return other.CoDiv(this);
        }

        public Integer Succ()
        {
            return new Integer(Value + 1);
        }

        public Integer Pred()
        {
            return new Integer(Value - 1);
        }

        // equality

        public Bool Equal(Integer other)
        {
            return Value == other.Value;
        }

        // ordering

        public Bool Less(Integer other)
        {
            return Value < other.Value;
        }
    }
}
