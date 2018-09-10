using System.Collections.Generic;
using CoTy.Ambiance;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public partial class Closure : Sequence
    {
        public Closure(AmScope lexical, params Cobject[] objs)
            : this(lexical, (IEnumerable<Cobject>) objs)
        {
        }
        public Closure(AmScope lexical, IEnumerable<Cobject> objs)
            : base(objs)
        {
            Lexical = lexical;
        }

        private AmScope Lexical { get; }

        public override void Apply(AmScope context, AmStack stack)
        {
            var localContext = new AmScope(Lexical, "local");
            foreach (var value in this)
            {
                value.Close(localContext, stack);
            }
        }
    }
}
