using System;

using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Builtin : CoObject<Action<AmScope, AmStack>>
    {
        private readonly Action<AmScope, AmStack> apply;

        public Builtin(Action<AmScope, AmStack> apply)
            : base(apply)
        {
            this.apply = apply;
        }

        public override void Apply(AmScope scope, AmStack stack)
        {
            this.apply(scope, stack);
        }

        public override void Eval(AmScope scope, AmStack stack)
        {
            this.apply(scope, stack);
        }
    }
}
