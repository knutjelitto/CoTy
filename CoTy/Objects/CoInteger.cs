namespace CoTy.Objects
{
    public partial class CoInteger : CoTuple<long>
    {
        public CoInteger(long value) : base(value)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
