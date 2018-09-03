namespace CoTy.Objects
{
    public partial class CoString
    {
        public CoString Add(CoString other)
        {
            return new CoString(this.Value + other.Value);
        }

        public CoBoolean EQ(CoString other)
        {
            return CoBoolean.From(Value == other.Value);
        }

        public CoBoolean NE(CoString other)
        {
            return CoBoolean.From(Value == other.Value);
        }

        public CoString Add(CoInteger other)
        {
            return new CoString(this.Value + other.Value);
        }

        public CoTuple Add(dynamic other)
        {
            return other.CoAdd(this);
        }
    }
}
