using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class BooleanModule : Module
    {
        public BooleanModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("bool?", Arity = 1)]
        private static void IsBool(AmScope scope, AmStack stack)
        {
            stack.Push(Bool.From(stack.Pop() is Bool));
        }

        [Builtin("true")]
        private static void True(AmScope scope, AmStack stack)
        {
            stack.Push(Bool.True);
        }

        [Builtin("false")]
        private static void False(AmScope scope, AmStack stack)
        {
            stack.Push(Bool.False);
        }

        [Builtin("not")]
        private static void Not(AmScope scope, AmStack stack)
        {
            stack.Push(stack.Pop<Bool>().Not());
        }

        [Builtin("and")]
        private static void And(AmScope scope, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 && b2));
        }
    }
}
