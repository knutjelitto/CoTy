namespace CoTy.Objects
{
    public abstract partial class CoTuple
    {
        public Bool NotEqual(dynamic other)
        {
            return ((dynamic)this).Equal(other).Not();
        }

        public Bool NotLess(dynamic other)
        {
            return ((dynamic)this).Less(other).Not();
        }

        public Bool Greater(dynamic other)
        {
            return NotEqual(other) && NotLess(other);
        }

        public Bool LessOrEqual(dynamic other)
        {
            return ((dynamic)this).Less(other) || ((dynamic)this).Equal(other);
        }

        public Bool GreaterOrEqual(dynamic other)
        {
            return ((dynamic)this).Greater(other) || ((dynamic)this).Equal(other);
        }
    }
}
