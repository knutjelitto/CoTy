using System;

using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Builtin : CoMutator
    {
        private readonly Action<AmScope, AmStack> apply;

        public Builtin(Action<AmScope, AmStack> apply)
        {
            this.apply = apply;
        }

        public override void Apply(AmScope scope, AmStack stack)
        {
            this.apply(scope, stack);
        }
    }
}
