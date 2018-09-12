using System.Collections.Generic;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public partial class Closure : Sequence
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

        public override void Apply(Context context, Stack stack)
        {
            var localContext = Lexical.Push("local");
            foreach (var value in this)
            {
                value.Close(localContext, stack);
            }
        }

        public override string ToString()
        {
            return "[" + string.Join(" ", Value) + "]";
        }
    }
}
