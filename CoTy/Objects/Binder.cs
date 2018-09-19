using System.Collections.Generic;
using System.Linq;
using CoTy.Support;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Binder : Cobject<List<Symbol>>
    {
        private Binder(IEnumerable<Symbol> objs)
            : base(objs.Reverse().ToList())
        {
        }

        public List<Symbol> Symbols => Value;

        public static Binder From(IEnumerable<Symbol> values)
        {
            return new Binder(values);
        }

        public static Binder From(params Symbol[] objs)
        {
            return From(objs.AsEnumerable());
        }

        public override void Lambda(IScope scope, IStack stack)
        {
            stack.Check(Value.Count);
            foreach (var symbol in Value)
            {
                var value = stack.Pop();
                scope.Define(symbol, value);
            }
        }

        public override void Apply(IScope scope, IStack stack)
        {
            // does nothing -- can't be applied
        }

        public override bool Equals(object obj)
        {
            return obj is Binder other && Symbols.SequenceEqual(other.Symbols);
        }

        public override int GetHashCode()
        {
            return Hash.Up(Symbols);
        }

        public override string ToString()
        {
            if (Value.Count == 1)
            {
                return $"{Symbol.Bind}{Value[0]}";
            }
            return $"{Symbol.Bind}({string.Join(" ", Value.AsEnumerable().Reverse())})";
        }
    }
}
