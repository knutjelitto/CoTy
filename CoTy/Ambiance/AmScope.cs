using System.Collections.Generic;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class AmScope : IDefine
    {
        private readonly Dictionary<Symbol, Binding> definitions = new Dictionary<Symbol, Binding>();

        public AmScope(AmScope parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public IEnumerable<Symbol> Symbols => this.definitions.Keys;

        public bool IsDefined(Symbol symbol)
        {
            return TryFind(symbol, out var _);
        }

        public bool CanDefine(Symbol symbol)
        {
            return !this.definitions.ContainsKey(symbol);
        }

        public void Define(Symbol symbol, Cobject cobject)
        {
            if (!TryDefine(symbol, cobject))
            {
                throw new ScopeException($"ill: can't define symbol `{symbol}´");
            }
        }

        private bool TryDefine(Symbol symbol, Cobject cobject)
        {
            if (CanDefine(symbol))
            {
                this.definitions.Add(symbol, new Binding(symbol, cobject, this));
                return true;
            }

            return false;
        }

        public void Find(Symbol symbol, out Binding binding)
        {
            if (!TryFind(symbol, out binding))
            {
                throw new ScopeException($"ill: can't find definition for symbol `{symbol}´");
            }
        }

        public bool TryFind(Symbol symbol, out Binding value)
        {
            if (!this.definitions.TryGetValue(symbol, out value))
            {
                if (Parent != null)
                {
                    return Parent.TryFind(symbol, out value);
                }

                value = null;
                return false;
            }

            return true;
        }

        public AmScope Parent { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name + "{" + string.Join(" ", Symbols) + "}";
        }
    }
}
