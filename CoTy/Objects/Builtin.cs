using System;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<IScope, IStack>>
    {
        private Builtin(Action<IScope, IStack> eval)
            : base(eval)
        {
        }

        public static Builtin From(Action<IScope, IStack> eval)
        {
            return new Builtin(eval);
        }

        public override void Lambda(IScope scope, IStack stack)
        {
            Value(scope, stack);
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
