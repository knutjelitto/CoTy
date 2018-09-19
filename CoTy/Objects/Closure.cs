using System.Collections.Generic;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Closure : Sequence
    {
        private Closure(IScope lexical, IEnumerable<object> objs)
            : base(objs)
        {
            Lexical = lexical;
        }

        private IScope Lexical { get; }

        public static Closure From(IScope scope, IEnumerable<object> values)
        {
            return new Closure(scope, values);
        }

        public static Closure From(IScope scope, params object[] objs)
        {
            return From(scope, (IEnumerable<object>)objs);
        }

        public override void Lambda(IScope scope, IStack stack)
        {
            stack.Push(this);
        }

        public override void Apply(IScope scope, IStack stack)
        {
            var localScope = Lexical.Chain(Binder.From("local"));
            foreach (var value in Value)
            {
                value.Lambda(localScope, stack);
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && obj is Closure other && Equals(Lexical, other.Lexical);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 17 * Lexical.GetHashCode();
        }

        public override string ToString()
        {
            return "[" + string.Join(" ", Value) + "]";
        }
    }
}
