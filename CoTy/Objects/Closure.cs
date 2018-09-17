using System.Collections.Generic;
using System.Linq;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Closure : Sequence
    {
        protected Closure(Context lexical, IEnumerable<object> objs)
            : base(objs)
        {
            Lexical = lexical;
        }

        private Context Lexical { get; }

        public static Closure From(Context context, IEnumerable<object> values)
        {
            return new Closure(context, values);
        }

        public static Closure From(Context context, params object[] objs)
        {
            return From(context, (IEnumerable<object>)objs);
        }

        public override void Close(Context context, Stack stack)
        {
            stack.Push(this);
        }

        public override void Apply(Context context, Stack stack)
        {
            context = Lexical.Push("local");
            foreach (var value in Value)
            {
                Close(context, stack, value);
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
