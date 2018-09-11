using System.Collections.Generic;
using CoTy.Ambiance;

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

        public override void Apply(Context context, AmStack stack)
        {
            var localContext = Lexical.Push("local");
            foreach (var value in this)
            {
                value.Close(localContext, stack);
            }
        }
    }
}
