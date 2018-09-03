// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class CoBoolean
    {
        public CoBoolean EQ(CoBoolean other)
        {
            return Value == other.Value;
        }

        public CoBoolean NE(CoBoolean other)
        {
            return Value != other.Value;
        }
    }
}
