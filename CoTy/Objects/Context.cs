using System.Collections.Generic;
using System.Linq;

using CoTy.Errors;

namespace CoTy.Objects
{
    public class Context : Cobject<Dictionary<Symbol, Binding>>, IScope
    {
        private Context(IScope parent, string name)
            : base(new Dictionary<Symbol, Binding>())
        {
            Parent = parent;
            Name = name;
        }

        public IEnumerable<Symbol> Symbols => Value.Keys.OrderBy(k => k.ToString());

        public static IScope Root(string name)
        {
            return new Context(null, name);
        }

        public IScope Pop()
        {
            Parent = null;
            return this;
        }

        public IScope Push(string name)
        {
            var newContext = new Context(this, name);
            return newContext;
        }

        public bool IsDefined(Symbol symbol)
        {
            return TryFind(symbol, out var _);
        }

        public void Define(string symbol, object value, bool isSealed = false, bool isOpaque = false)
        {
            Define(Symbol.Get(symbol), value, isSealed, isOpaque);
        }

        public void Define(Symbol symbol, object value, bool isSealed = false, bool isOpaque = false)
        {
            if (TryFind(symbol, out var binding))
            {
                if (Equals(binding.Scope, this))
                {
                    throw new BinderException($"`{symbol}´ already defined in current scope");
                }

                if (binding.IsOpaque)
                {
                    throw new BinderException($"`{symbol}´ is marked as opaque and can't be redefined");
                }
            }

            binding = new Binding(this, value, isSealed, isOpaque);
            Value.Add(symbol, binding);
        }

        public void Update(Symbol symbol, object value)
        {
            var binding = Find(symbol);

            if (binding.IsSealed)
            {
                throw new BinderException($"`{symbol}´ is marked as sealed and can't be updated");
            }

            binding.Value = value;
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


        public void Get(Symbol symbol, out object value)
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

        public IScope Parent { get; private set; }
        public string Name { get; }

        public override bool Equals(object obj)
        {
            return obj is Context other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Name + "{" + string.Join(" ", Symbols) + "}";
        }
    }
}
