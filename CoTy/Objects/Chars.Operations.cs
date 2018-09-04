namespace CoTy.Objects
{
    public partial class Chars : IComparable<Chars>, IOrdered<Chars>
    {
        // equality

        public Bool Equal(Chars other)
        {
            return Bool.From(Value == other.Value);
        }

        // ordering

        public Bool Less(Chars other)
        {
            return Bool.From(Value.CompareTo(other.Value) < 0);
        }

        public Chars Add(Chars other)
        {
            return new Chars(this.Value + other.Value);
        }

        public Chars Add(Integer other)
        {
            return new Chars(this.Value + other.Value);
        }

        public Cobject Add(dynamic other)
        {
            return other.CoAdd(this);
        }
    }
}
