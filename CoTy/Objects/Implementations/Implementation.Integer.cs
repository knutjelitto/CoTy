// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable MemberCanBePrivate.Global
namespace CoTy.Objects.Implementations
{
    public partial class Implementation
    {
        public Integer Zero(Integer value)
        {
            return Integer.Zero;
        }

        public Integer Plus(Integer value1, Integer value2)
        {
            return Integer.From(value1.Value + value2.Value);
        }

        public Integer Minus(Integer value1, Integer value2)
        {
            return Integer.From(value1.Value - value2.Value);
        }

        public Integer Star(Integer value1, Integer value2)
        {
            return Integer.From(value1.Value * value2.Value);
        }

        public Integer Slash(Integer value1, Integer value2)
        {
            return Integer.From(value1.Value / value2.Value);
        }

        public Integer Succ(Integer value)
        {
            return Integer.From(value.Value + 1);
        }

        public Integer Pred(Integer value)
        {
            return Integer.From(value.Value - 1);
        }
    }
}
