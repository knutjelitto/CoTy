namespace CoTy.Objects
{
    public partial class Chars : Cobject<string>
    {
        public Chars(string value) : base(value)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Chars other && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return "\"" + Value.Replace("\"", "\\\"") + "\"";
        }
    }
}
