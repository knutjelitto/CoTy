namespace CoTy.Objects
{
    public class CoString : CoSelfish<string>
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
