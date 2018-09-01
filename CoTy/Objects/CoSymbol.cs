using System.Collections.Generic;
using CoTy.Ambiance;

namespace CoTy.Objects
{
    public class CoSymbol : CoObject<string>
    {
        private static readonly Dictionary<string, CoSymbol> symbols = new Dictionary<string, CoSymbol>();

        public static readonly CoSymbol True = Get("true");
        public static readonly CoSymbol False = Get("false");
        public static readonly CoSymbol End = Get("end");
        public static readonly CoSymbol LeftParent = Get("(");
        public static readonly CoSymbol RightParent = Get(")");
        public static readonly CoSymbol Define = Get("define");

        static CoSymbol()
        {
            symbols = new Dictionary<string, CoSymbol>();
        }

        private readonly int hashCode;

        private CoSymbol(string value) : base(value)
        {
            this.hashCode = value.GetHashCode();
        }

        public static CoSymbol Get(string name)
        {
            if (!symbols.TryGetValue(name, out var symbol))
            {
                symbol = new CoSymbol(name);
                symbols.Add(name, symbol);
            }

            return symbol;
        }

        public override int GetHashCode() => this.hashCode;

        public override void Apply(AmScope scope, AmStack stack)
        {
            var value = scope.Find(this);
            value.Apply(scope, stack);
        }

        public override bool Equals(object obj)
        {
            return obj is CoSymbol other && this.Value == other.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
