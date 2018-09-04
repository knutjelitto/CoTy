using System.Collections.Generic;
using CoTy.Ambiance;
using CoTy.Errors;

namespace CoTy.Objects
{
    public class Symbol : CoTuple<string>
    {
        private static readonly Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public static readonly Symbol True = Get("true");
        public static readonly Symbol False = Get("false");
        public static readonly Symbol End = Get("end");
        public static readonly Symbol Quoter = Get("'");
        public static readonly Symbol LeftParent = Get("(");
        public static readonly Symbol RightParent = Get(")");
        public static readonly Symbol Define = Get("define");

        static Symbol()
        {
            symbols = new Dictionary<string, Symbol>();
        }

        private readonly int hashCode;

        private Symbol(string value) : base(value)
        {
            this.hashCode = value.GetHashCode();
        }

        public static Symbol Get(string name)
        {
            if (!symbols.TryGetValue(name, out var symbol))
            {
                symbol = new Symbol(name);
                symbols.Add(name, symbol);
            }

            return symbol;
        }

        public override void Apply(AmScope scope, AmStack stack)
        {
            if (!scope.TryFind(this, out var definition))
            {
                throw new ScopeException($"ill: can't find definition for symbol `{this}´");
            }

            definition.Eval(scope, stack);
        }

        public override int GetHashCode() => this.hashCode;

        public override bool Equals(object obj)
        {
            return obj is Symbol other && this.Value == other.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
