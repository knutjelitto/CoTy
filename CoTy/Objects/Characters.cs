namespace CoTy.Objects
{
    public class Characters : Cobject<string>
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
