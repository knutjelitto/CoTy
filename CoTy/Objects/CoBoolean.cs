namespace CoTy.Objects
{
    public class CoBoolean : CoSelfish<bool>
    {
        public static readonly CoBoolean True = new CoBoolean(true);
        public static readonly CoBoolean False = new CoBoolean(false);

        private CoBoolean(bool value) : base(value)
        {
        }

        public override string ToString()
        {
            return Value ? CoSymbol.True.ToString() : CoSymbol.False.ToString();
        }
    }
}
