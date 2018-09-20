using System;

namespace CoTy.Objects
{
    public class Builtin : Cobject<Action<IScope, IStack>>
    {

        private Builtin(Symbol name, Action<IScope, IStack> eval)
            : base(eval)
        {
            Name = name;
        }

        public Symbol Name { get; }

        public static Builtin From(Symbol name, Action<IScope, IStack> eval)
        {
            return new Builtin(name, eval);
        }

        public override void Eval(IScope scope, IStack stack)
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
            return $"{Name}:{Value}";
        }
    }
}
