using System;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<Context, AmStack>, Builtin>
    {
        private readonly Action<Context, AmStack> eval;

        public Builtin(Action<Context, AmStack> eval)
            : base(eval)
        {
            this.eval = eval;
        }

        public override void Close(Context context, AmStack stack)
        {
            this.eval(context, stack);
        }
    }
}
