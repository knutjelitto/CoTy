namespace CoTy.Objects
{
    public partial class Characters : Cobject<string, Characters>
    {
        public Characters(string value) : base(value)
        {
        }

        public static implicit operator Characters(string characters)
        {
            return new Characters(characters);
        }

        public override bool Equals(object obj)
        {
            return obj is Characters other && Value.Equals(other.Value);
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
