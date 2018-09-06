using System;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<IContext, AmStack>>
    {
        private readonly Action<IContext, AmStack> eval;

        public Builtin(Action<IContext, AmStack> eval)
            : base(eval)
        {
            this.eval = eval;
        }

        public override void Eval(IContext context, AmStack stack)
        {
            this.eval(context, stack);
        }
    }
}
