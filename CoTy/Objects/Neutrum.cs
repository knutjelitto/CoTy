namespace CoTy.Objects
{
    public class Neutrum : Cobject
    {
        public static readonly Neutrum Neutral = new Neutrum();

        private Neutrum()
        {
        }

        public static string operator +(Neutrum neutral, string value)
        {
            return string.Empty + value;
        }

        public static Integer operator +(Neutrum neutral, Integer value)
        {
            return value;
        }

        public static Integer operator -(Neutrum neutral, Integer value)
        {
            return - value;
        }

        public static Integer operator *(Neutrum neutral, Integer value)
        {
            return value;
        }

        public static Integer operator /(Neutrum neutral, Integer value)
        {
            return 1 / value;
        }

        public override string ToString()
        {
            return "neutral";
        }
    }
}
