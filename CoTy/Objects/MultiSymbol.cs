using System;
using System.Collections.Generic;
using System.Linq;

using CoTy.Support;

namespace CoTy.Objects
{
    public abstract class MultiSymbol : Cobject<List<Symbol>>
    {
        private readonly Symbol literal;
        private readonly Action<IScope, Symbol, object> action;

        protected MultiSymbol(IEnumerable<Symbol> objs, Symbol literal, Action<IScope, Symbol, object> action)
            : base(objs.Reverse().ToList())
        {
            this.literal = literal;
            this.action = action;
        }

        private List<Symbol> Symbols => Value;

        public override void Eval(IScope scope, IStack stack)
        {
            stack.Check(Symbols.Count);
            foreach (var symbol in Symbols)
            {
                var value = stack.Pop();
                this.action(scope, symbol, value);
            }
        }

        public override bool Equals(object obj)
        {
            return GetType() == obj.GetType() && obj is MultiSymbol other && Symbols.SequenceEqual(other.Symbols);
        }

        public override int GetHashCode()
        {
            return Hash.Up(Symbols);
        }

        public override string ToString()
        {
            if (Value.Count == 1)
            {
                return $"{this.literal}{Value[0]}";
            }
            return $"{this.literal}({string.Join(" ", Value.AsEnumerable().Reverse())})";
        }
    }
}
