using System;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<IContext, IStack>>
    {
        private Builtin(Action<IContext, IStack> eval)
            : base(eval)
        {
        }

        public static Builtin From(Action<IContext, IStack> eval)
        {
            return new Builtin(eval);
        }

        public override void Lambda(IContext context, IStack stack)
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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
