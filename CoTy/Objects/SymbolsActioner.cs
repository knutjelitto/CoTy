using System.Collections.Generic;
using System.Linq;

using CoTy.Support;

namespace CoTy.Objects
{
    public abstract class SymbolsActioner : Cobject<List<Symbol>>
    {
        private readonly Symbol literal;

        protected SymbolsActioner(IEnumerable<Symbol> objs, Symbol literal)
            : base(objs.Reverse().ToList())
        {
            this.literal = literal;
        }

        private List<Symbol> Symbols => Value;

        protected abstract void ActionOnValue(IScope scope, Symbol symbol, object value);

        public override void Eval(IScope scope, IStack stack)
        {
            stack.Check(Symbols.Count);
            foreach (var symbol in Symbols)
            {
                var value = stack.Pop();
                ActionOnValue(scope, symbol, value);
            }
        }

        public override bool Equals(object obj)
        {
            return GetType() == obj.GetType() && obj is SymbolsActioner other && Symbols.SequenceEqual(other.Symbols);
        }

        public override int GetHashCode()
        {
            return Hash.Up(Symbols);
        }

        public override string ToString()
        {
            if (Value.Count == 1)
            {
                return $"{this.literal} {Value[0]}";
            }
            return $"{this.literal} ({string.Join(" ", Value.AsEnumerable().Reverse())})";
        }
    }
}
