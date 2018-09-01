using System.Collections.Generic;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class AmScope
    {
        private readonly Dictionary<CoSymbol, CoObject> definitions = new Dictionary<CoSymbol, CoObject>();

        public AmScope(AmScope parent)
        {
            Parent = parent;
        }

        public virtual void Define(CoSymbol symbol, CoObject @object)
        {
            if (!TryDefine(symbol, @object))
            {
                throw new ScopeException($"ill: can't define symbol `{symbol}´");
            }
        }

        public virtual bool TryDefine(CoSymbol symbol, CoObject @object)
        {
            if (!this.definitions.ContainsKey(symbol))
            {
                this.definitions.Add(symbol, @object);
                return true;
            }

            return false;
        }

        public virtual CoObject Find(CoSymbol symbol)
        {
            if (!TryFind(symbol, out var @object))
            {
                throw new ScopeException($"ill: can't find definition for symbol `{symbol}´");
            }

            return @object;
        }

        public virtual bool TryFind(CoSymbol symbol, out CoObject @object)
        {
            if (!this.definitions.TryGetValue(symbol, out @object))
            {
                if (Parent != null)
                {
                    return Parent.TryFind(symbol, out @object);
                }
            }
            return true;
        }

        public AmScope Parent { get; }
    }
}
