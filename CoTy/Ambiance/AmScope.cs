using System.Collections.Generic;
using System.Linq;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class AmScope
    {
        private readonly Dictionary<Symbol, Binding> definitions = new Dictionary<Symbol, Binding>();

        public AmScope(AmScope parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public IEnumerable<Symbol> Symbols => this.definitions.Keys.OrderBy(k => k.Value);

        public AmScope Scope => this;

        public AmScope Pop(out AmScope popped)
        {
            var result = Parent;
            Parent = null;
            popped = this;
            return result;
        }

        public AmScope Push(AmScope pushed)
        {
            pushed.Parent = this;
            return pushed;
        }

        public bool IsDefined(Symbol symbol)
        {
            return TryFind(symbol, out var _);
        }

        public bool CanDefine(Symbol symbol)
        {
            return !this.definitions.ContainsKey(symbol);
        }

        public bool CanUpdate(Symbol symbol)
        {
            return TryFind(symbol, out var binding) && !binding.IsSealed;
        }

        public void Define(Symbol symbol, Cobject value, bool isSealed = false, bool isOpaque = false)
        {
            if (TryFind(symbol, out var binding))
            {
                if (binding.Scope == this)
                {
                    throw new BinderException($"`{symbol}´ already defined in current scope");
                }

                if (binding.IsOpaque)
                {
                    throw new BinderException($"`{symbol}´ is marked as opaque and can't be redefined");
                }
            }

            binding = new Binding(this, symbol, value, isSealed, isOpaque);
            this.definitions.Add(symbol, binding);
        }

        public void Undefine(Symbol symbol)
        {
            var binding = Find(symbol);

            if (binding.IsOpaque)
            {
                throw new BinderException($"`{symbol}´ is marked as opaque and can't be removed");
            }

            if (binding.IsSealed)
            {
                throw new BinderException($"`{symbol}´ is marked as sealed and can't be removed");
            }

            binding.Scope.definitions.Remove(symbol);
        }

        public void Update(Symbol symbol, Cobject value)
        {
            var binding = Find(symbol);

            if (binding.IsSealed)
            {
                throw new BinderException($"`{symbol}´ is marked as sealed and can't be updated");
            }

            binding.Value = value;
        }

        public void Get(Symbol symbol, out Cobject value)
        {
            value = Find(symbol).Value;
        }

        public Binding Find(Symbol symbol)
        {
            if (!TryFind(symbol, out var binding))
            {
                throw new BinderException($"`{symbol}´ isn't defined in any scope");
            }

            return binding;
        }

        public bool TryFind(Symbol symbol, out Binding binding)
        {
            if (!this.definitions.TryGetValue(symbol, out binding))
            {
                if (Parent != null)
                {
                    return Parent.TryFind(symbol, out binding);
                }

                binding = null;
                return false;
            }

            return true;
        }

        public AmScope Parent { get; set; }
        public string Name { get; }

        public override string ToString()
        {
            return Name + "{" + string.Join(" ", Symbols) + "}";
        }
    }
}
