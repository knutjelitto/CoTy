using System.Collections.Generic;
using System.Linq;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Closure : Sequence
    {
        public Closure(Context lexical, params Cobject[] objs)
            : this(lexical, (IEnumerable<Cobject>) objs)
        {
        }

        public Closure(Context lexical, IEnumerable<Cobject> objs)
            : base(objs)
        {
            Lexical = lexical;
        }

        private Context Lexical { get; }

        protected override void Close(Context context, Stack stack)
        {
            //stack.Push(new Closure(context, Value));
            stack.Push(this);
        }

        protected override void Apply(Context context, Stack stack)
        {
            context = Lexical.Push("local");
            foreach (var value in Value)
            {
                Close(context, stack, value);
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && Equals(Lexical, ((Closure)obj).Lexical);
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
