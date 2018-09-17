using System.Collections.Generic;
using System.Linq;
using CoTy.Errors;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Binder : Cobject<List<Symbol>>
    {
        private Binder(IEnumerable<Symbol> objs)
            : base(objs.ToList())
        {
        }

        public static Binder From(IEnumerable<Symbol> values)
        {
            return new Binder(values);
        }

        public static Binder From(params Symbol[] objs)
        {
            return From(objs.AsEnumerable());
        }

        public override void Close(Context context, Stack stack)
        {
            if (Value.Count > stack.Count)
            {
                throw new StackException(Value.Count, stack.Count);
            }

            foreach (var symbol in Value.AsEnumerable().Reverse())
            {
                var value = stack.Pop();
                context.Define(symbol, value);
            }
        }

        public override void Apply(Context context, Stack stack)
        {
            // does nothing -- can't be applied
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && obj is Binder;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            if (Value.Count == 1)
            {
                return ":" + Value[0];
            }
            return ":(" + string.Join(" ", Value) + ")";
        }
    }
}
