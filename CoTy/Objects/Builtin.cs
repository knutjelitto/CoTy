using System;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<AmScope, AmStack>, Builtin>
    {
        private readonly Action<AmScope, AmStack> eval;

        public Builtin(Action<AmScope, AmStack> eval)
            : base(eval)
        {
            this.eval = eval;
        }

        public override void Close(AmScope context, AmStack stack)
        {
            this.eval(context, stack);
        }
    }
}
