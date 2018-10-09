using System.Collections.Generic;
using CoTy.Errors;

namespace CoTy.Objects
{
    public class Binder : Cobject, IBinder
    {
        private readonly Dictionary<Symbol, Binding> bindings = new Dictionary<Symbol, Binding>();
        private readonly List<Symbol> symbols = new List<Symbol>();

        private Binder(Symbol name)
        {
            Name = name;
        }

        public Symbol Name { get; }

        public IEnumerable<Symbol> Symbols => this.symbols;

        public static Binder From(Symbol name)
        {
            return new Binder(name);
        }

        public void Define(Symbol symbol, Cobject value, bool isSealed = false, bool isOpaque = false)
        {
            if (TryFind(symbol, out var binding))
            {
                throw new BinderException($"`{symbol}´ already defined in current scope");
            }

            binding = new Binding(this, value, Bool.From(isSealed), Bool.From(isOpaque));
            this.bindings.Add(symbol, binding);
            this.symbols.Add(symbol);
        }

        public void GetValue(Symbol symbol, out Cobject value)
        {
            value = Find(symbol).Value;
        }

        public bool IsDefined(Symbol symbol)
        {
            return TryFind(symbol, out var _);
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
            return this.bindings.TryGetValue(symbol, out binding);
        }

        public void Undefine(Symbol symbol)
        {
            var binding = Find(symbol);

            if (binding.IsOpaque.Value)
            {
                throw new BinderException($"`{symbol}´ is marked as opaque and can't be removed");
            }

            this.bindings.Remove(symbol);
            this.symbols.Remove(symbol);
        }

        public void Update(Symbol symbol, Cobject value)
        {
            var binding = Find(symbol);

            if (binding.IsSealed.Value)
            {
                throw new BinderException($"`{symbol}´ is marked as sealed and can't be updated");
            }

            binding.Value = value;
        }


        public override string ToString()
        {
            return Name + "{" + string.Join(" ", Symbols) + "}";
        }
    }
}
