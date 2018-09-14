using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using CoTy.Errors;

namespace CoTy.Objects
{
    public class Context : Cobject<Dictionary<Symbol, Context.Binding>>
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

        public void Update(Symbol symbol, object value)
        {
            var binding = Find(symbol);

            if (binding.IsSealed)
            {
                throw new BinderException($"`{symbol}´ is marked as sealed and can't be updated");
            }

            binding.Value = value;
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

        public Context Parent { get; private set; }
        public string Name { get; }

        public override bool Equals(object obj)
        {
            return obj is Table other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Name + "{" + string.Join(" ", Symbols) + "}";
        }

        public class Binding
        {
            public Binding(Context scope, object value, bool isSealed, bool isOpaque)
            {
                Scope = scope;
                Value = value;
                IsSealed = isSealed;
                IsOpaque = isOpaque;
            }

            public Context Scope { get; }
            public object Value { get; set; }
            public bool IsSealed { get; }
            public bool IsOpaque { get; }
        }

        private class Ctx : Cobject<(Dictionary<Symbol, Binding> Dict, Stack Stack)>
        {
            public Ctx()
                : this(new Dictionary<Symbol, Binding>(), new Stack())
            {
            }
            private Ctx(Dictionary<Symbol, Binding> dict, Stack stack)
                : base((dict, stack))
            {
            }

            public Ctx WithStack(Stack stack)
            {
                return new Ctx(Value.Dict, stack);
            }

            public IEnumerable<object> Components()
            {
                var tuple = Value.ToTuple() as ITuple;
                for (var i = 0; i < tuple.Length; ++i)
                {
                    yield return tuple[i];
                }
            }
        }
    }
}
