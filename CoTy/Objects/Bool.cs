namespace CoTy.Objects
{
    public partial class Bool : Cobject<bool>
    {
        public static readonly Bool True = new Bool(true);
        public static readonly Bool False = new Bool(false);

        private Bool(bool value) : base(value)
        {
        }

        public Bool Not()
        {
            return Value ? False : True;
        }

        public static implicit operator Bool(bool value)
        {
            return value ? True : False;
        }

        public static implicit operator bool(Bool value)
        {
            return value.Value;
        }

        public static Bool From(bool value)
        {
            return value ? True : False;
        }

        public override string ToString()
        {
            return Value ? Symbol.True.ToString() : Symbol.False.ToString();
        }
    }
}
