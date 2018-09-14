using System;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<Context, Stack>>
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

        public override bool Equals(object obj)
        {
            return obj is Builtin other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
