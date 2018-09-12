using System;

namespace CoTy.Objects
{
    public partial class Integer : Cobject<int, Integer>
    {
        public static Integer Zero = new Integer(0);
        public static Integer One = new Integer(1);

        private Integer(int value) : base(value)
        {
        }

        public static Integer From(int value)
        {
            return new Integer(value);
        }

        public static bool TryFrom(string str, out Integer value)
        {
            if (int.TryParse(str, out var parsed))
            {
                value = new Integer(parsed);
                return true;
            }
            value = null;
            return false;
        }

        public int CompareTo(Integer other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
