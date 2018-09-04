// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Bool : IComparable<Bool>, IOrdered<Bool>
    {
        public Bool Equal(Bool other)
        {
            return Value == other.Value;
        }

        public Bool Less(Bool other)
        {
            return !Value && other.Value;
        }
    }
}
