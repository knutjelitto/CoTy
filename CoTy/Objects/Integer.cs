namespace CoTy.Objects
{
    public partial class Integer : CoTuple<long>, IComparable<Integer>, IOrdered<Integer>
    {
        private Integer(long value) : base(value)
        {
        }

        public static Integer From(string str)
        {
            return new Integer(long.Parse(str));
        }

        public static bool TryFrom(string str, out Integer value)
        {
            if (long.TryParse(str, out var parsed))
            {
                value = new Integer(parsed);
                return true;
            }
            value = null;
            return false;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
