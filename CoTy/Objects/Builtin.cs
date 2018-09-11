using System;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<Context, Stack>, Builtin>
    {
        private readonly Action<Context, Stack> eval;

        public Builtin(Action<Context, Stack> eval)
            : base(eval)
        {
            this.eval = eval;
        }

        public override void Close(Context context, Stack stack)
        {
            this.eval(context, stack);
        }
    }
}
