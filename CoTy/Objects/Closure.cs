using System.Collections.Generic;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Closure : Sequence
    {
        private Closure(IContext lexical, IEnumerable<object> objs)
            : base(objs)
        {
            Lexical = lexical;
        }

        private IContext Lexical { get; }

        public static Closure From(IContext context, IEnumerable<object> values)
        {
            return new Closure(context, values);
        }

        public static Closure From(IContext context, params object[] objs)
        {
            return From(context, (IEnumerable<object>)objs);
        }

        public override void Lambda(IContext context, IStack stack)
        {
            stack.Push(this);
        }

        public override void Apply(IContext context, IStack stack)
        {
            context = Lexical.Push("local");
            foreach (var value in Value)
            {
                value.Lambda(context, stack);
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
