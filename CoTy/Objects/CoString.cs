namespace CoTy.Objects
{
    public class CoString : CoObject<string>
    {
        public CoString(string value) : base(value)
        {
        }

        public CoString Add(CoString other)
        {
            return new CoString(this.Value + other.Value);
        }

        public CoString Add(CoInteger other)
        {
            return new CoString(this.Value + other.Value.ToString());
        }

        public CoObject Add(dynamic other)
        {
            return other.CoAdd(this);
        }

        public override string ToString()
        {
            return "\"" + Value.Replace("\"", "\\\"") + "\"";
        }
    }
}
