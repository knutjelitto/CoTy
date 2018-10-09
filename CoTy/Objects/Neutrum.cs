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

        public static object operator +(object value, Neutrum neutral)
        {
            return value;
        }

        public static object operator *(Neutrum neutral, object value)
        {
            return value;
        }

        public static object operator *(object value, Neutrum neutral)
        {
            return value;
        }

        public override string ToString()
        {
            return "<neutral>";
        }
    }
}
