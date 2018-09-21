using System.Linq;

namespace CoTy.Objects
{
    public class Neutrum : Cobject
    {
        public static readonly Neutrum Neutral = new Neutrum();

        private Neutrum()
        {
        }

        public static object operator +(Neutrum neutral, object value)
        {
            return value;
        }

        public static object operator *(Neutrum neutral, object value)
        {
            return value;
        }

        public static Integer operator -(Neutrum neutral, Integer value)
        {
            return -value;
        }

        public static object operator -(Neutrum neutral, string value)
        {
            return string.Join(string.Empty, value.Reverse());
        }

        public static object operator /(Neutrum neutral, Integer value)
        {
            return 1 / value;
        }

        public override string ToString()
        {
            return "<neutral>";
        }
    }
}
