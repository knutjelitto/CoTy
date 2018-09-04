using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class BoolModule : Module
    {
        [Builtin("bool?", Arity = 1)]
        private static void IsBool(AmScope scope, AmStack stack)
        {
            stack.Push(Bool.From(stack.Pop() is Bool));
        }

        [Builtin("true", Arity = 0)]
        private static void True(AmScope scope, AmStack stack)
        {
            stack.Push(Bool.True);
        }

        [Builtin("false", Arity = 0)]
        private static void False(AmScope scope, AmStack stack)
        {
            stack.Push(Bool.False);
        }

        [Builtin("not", Arity = 1)]
        private static void Not(AmScope scope, AmStack stack)
        {
            stack.Push(stack.Pop<Bool>().Not());
        }

        [Builtin("and", Arity = 2)]
        private static void And(AmScope scope, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 && b2));
        }

        [Builtin("or", Arity = 2)]
        private static void Or(AmScope scope, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 || b2));
        }
    }
}
