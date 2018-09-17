using System;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<Context, Stack>>
    {
        private Builtin(Action<Context, Stack> eval)
            : base(eval)
        {
        }

        public static Builtin From(Action<Context, Stack> eval)
        {
            return new Builtin(eval);
        }

        public override void Close(Context context, Stack stack)
        {
            Value(context, stack);
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
