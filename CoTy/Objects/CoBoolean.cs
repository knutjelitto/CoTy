namespace CoTy.Objects
{
    public partial class CoBoolean : CoTuple<bool>
    {
        public static readonly CoBoolean True = new CoBoolean(true);
        public static readonly CoBoolean False = new CoBoolean(false);

        private CoBoolean(bool value) : base(value)
        {
        }

        public static implicit operator CoBoolean(bool value)
        {
            return value ? True : False;
        }

        public static implicit operator bool(CoBoolean value)
        {
            return value.Value;
        }

        public static CoBoolean From(bool value)
        {
            return value ? True : False;
        }

        public override string ToString()
        {
            return Value ? CoSymbol.True.ToString() : CoSymbol.False.ToString();
        }
    }
}
