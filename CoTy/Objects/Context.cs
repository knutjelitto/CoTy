using System.Collections.Generic;
using System.Linq;
using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Objects
{
    public class Context : Cobject<Dictionary<Symbol, Binding>, Context>
    {
        protected Context(Context parent, string name)
            : base(new Dictionary<Symbol, Binding>())
        {
            Parent = parent;
            Name = name;
        }

        public IEnumerable<Symbol> Symbols => Value.Keys.OrderBy(k => k.Value);

        public Context Scope => this;

        public static Context Root(string name)
        {
            return new Context(null, name);
        }

        public Context Pop()
        {
            Parent = null;
            return this;
        }

        public Context Push(string name)
        {
            var newContext = new Context(this, name);
            return newContext;
        }

        public bool IsDefined(Symbol symbol)
        {
            return TryFind(symbol, out var _);
        }

        public bool CanDefine(Symbol symbol)
        {
            return !Value.ContainsKey(symbol);
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
            Value.Add(symbol, binding);
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

            binding.Scope.Value.Remove(symbol);
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
            if (!Value.TryGetValue(symbol, out binding))
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

        public Context Parent { get; private set; }
        public string Name { get; }

        public override string ToString()
        {
            return Name + "{" + string.Join(" ", Symbols) + "}";
        }
    }
}
