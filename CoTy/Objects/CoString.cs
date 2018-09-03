namespace CoTy.Objects
{
    public partial class CoString : CoTuple<string>
    {
        public CoString(string value) : base(value)
        {
        }

        public override string ToString()
        {
            return "\"" + Value.Replace("\"", "\\\"") + "\"";
        }
    }
}
