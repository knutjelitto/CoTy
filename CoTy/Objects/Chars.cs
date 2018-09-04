namespace CoTy.Objects
{
    public partial class Chars : CoTuple<string>
    {
        public Chars(string value) : base(value)
        {
        }

        public override string ToString()
        {
            return "\"" + Value.Replace("\"", "\\\"") + "\"";
        }
    }
}
