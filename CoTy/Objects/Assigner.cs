using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Sources;
using CoTy.Support;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Assigner : Cobject<List<Symbol>>
    {
        private Assigner(IEnumerable<Symbol> objs)
            : base(objs.Reverse().ToList())
        {
        }

        public List<Symbol> Symbols => Value;

        public static Assigner From(IEnumerable<Symbol> values)
        {
            return new Assigner(values);
        }

        public static Assigner From(params Symbol[] objs)
        {
            return From(objs.AsEnumerable());
        }

        public override void Lambda(IScope scope, IStack stack)
        {
            stack.Check(Symbols.Count);
            foreach (var symbol in Symbols)
            {
                var value = stack.Pop();
                scope.Update(symbol, value);
            }
        }

        public override void Apply(IScope scope, IStack stack)
        {
            // does nothing -- can't be applied
        }

        public override bool Equals(object obj)
        {
            return obj is Assigner other && Symbols.SequenceEqual(other.Symbols);
        }

        public override int GetHashCode()
        {
            return Hash.Up(Symbols);
        }

        public override string ToString()
        {
            if (Value.Count == 1)
            {
                return $"{Symbol.Assign}{Value[0]}";
            }
            return $"{Symbol.Assign}({string.Join(" ", Value.AsEnumerable().Reverse())})";
        }
    }
}
