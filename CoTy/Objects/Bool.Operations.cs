// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Bool : IOrdered<Bool>
    {
        public Bool Less(Bool other)
        {
            return !Value && other.Value;
        }
    }
}
