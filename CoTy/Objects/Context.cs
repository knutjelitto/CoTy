using System.Collections.Generic;
using CoTy.Errors;

namespace CoTy.Objects
{
    public class Context : Cobject, IScope
    {
        private readonly Context Parent;
        private readonly IBinder Binder;

        private Context(Context parent, IBinder binder)
        {
            this.Parent = parent;
            this.Binder = binder;
        }

        public IEnumerable<IBinder> Binders
        {
            get
            {
                var current = this;
                while (current != null)
                {
                    yield return current.Binder;

                    current = current.Parent;
                }

            }
        }

        public static IScope Root(IBinder binder)
        {
            return new Context(null, binder);
        }

        public IScope Chain(IBinder binder)
        {
            return new Context(this, binder);
        }

        public bool IsDefined(Symbol symbol)
        {
            return TryFind(symbol, out var _);
        }

        public void Define(Symbol symbol, object value, bool isSealed = false, bool isOpaque = false)
        {
            if (TryFind(symbol, out var binding))
            {
                if (Equals(binding.Binder, this.Binder))
                {
                    throw new BinderException($"`{symbol}´ already defined in current scope");
                }

                if (binding.IsOpaque)
                {
                    throw new BinderException($"`{symbol}´ is marked as opaque and can't be redefined");
                }
            }

            this.Binder.Define(symbol, value, isSealed, isOpaque);
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

            binding.Binder.Undefine(symbol);
        }


        public void GetValue(Symbol symbol, out object value)
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
            if (!this.Binder.TryFind(symbol, out binding))
            {
                if (this.Parent != null)
                {
                    return this.Parent.TryFind(symbol, out binding);
                }

                binding = null;
                return false;
            }

            return true;
        }
    }
}
