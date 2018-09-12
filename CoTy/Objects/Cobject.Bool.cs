// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Cobject : ICompare<Bool>
    {
        public int? Compare(Bool value1, Bool value2)
        {
            return value1.Value ? (value2.Value ? 0 : 1) : (value2 ? -1 : 0);
        }

        public Bool Not(Bool value)
        {
            return value.Not();
        }

        public Bool And(Bool value1, Bool value2)
        {
            return value1.Value & value2.Value;
        }

        public Bool Or(Bool value1, Bool value2)
        {
            return value1.Value | value2.Value;
        }
    }
}
