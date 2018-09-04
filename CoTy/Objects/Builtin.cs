using System;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public class Builtin : CoTuple<Action<AmScope, AmStack>>
    {
        private readonly Action<AmScope, AmStack> eval;

        public Builtin(Action<AmScope, AmStack> eval)
            : base(eval)
        {
            this.eval = eval;
        }

        public override void Eval(AmScope scope, AmStack stack)
        {
            this.eval(scope, stack);
        }
    }
}
