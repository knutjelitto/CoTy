// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Cobject
    {
        public int? Compare(Bool value1, Bool value2)
        {
            return value1.Value ? (value2.Value ? 0 : 1) : (value2 ? -1 : 0);
        }
    }
}
