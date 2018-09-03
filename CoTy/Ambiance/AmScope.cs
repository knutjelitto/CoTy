using System.Collections.Generic;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class AmScope
    {
        private readonly Dictionary<CoSymbol, Binding> definitions = new Dictionary<CoSymbol, Binding>();

        public AmScope(AmScope parent)
        {
            Parent = parent;
        }

        public virtual void Define(CoSymbol symbol, CoTuple @object)
        {
            if (!TryDefine(symbol, @object))
            {
                throw new ScopeException($"ill: can't define symbol `{symbol}´");
            }
        }

        public virtual bool TryDefine(CoSymbol symbol, CoTuple value)
        {
            if (!this.definitions.ContainsKey(symbol))
            {
                this.definitions.Add(symbol, new Binding(symbol, value));
                return true;
            }

            return false;
        }

        public virtual CoTuple Find(CoSymbol symbol)
        {
            if (!TryFind(symbol, out var @object))
            {
                throw new ScopeException($"ill: can't find definition for symbol `{symbol}´");
            }

            return @object;
        }

        public virtual bool TryFind(CoSymbol symbol, out CoTuple value)
        {
            if (!this.definitions.TryGetValue(symbol, out var binding))
            {
                if (Parent != null)
                {
                    return Parent.TryFind(symbol, out value);
                }

                value = null;
                return false;
            }

            value = binding.Value;
            return true;
        }

        public AmScope Parent { get; }
    }
}
