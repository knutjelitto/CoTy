using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class BoolModule : Module
    {
        [Builtin("bool?", InArity = 1)]
        private static void IsBool(IContext context, AmStack stack)
        {
            stack.Push(Bool.From(stack.Pop() is Bool));
        }

        [Builtin("true", InArity = 0)]
        private static void True(IContext context, AmStack stack)
        {
            stack.Push(Bool.True);
        }

        [Builtin("false", InArity = 0)]
        private static void False(IContext context, AmStack stack)
        {
            stack.Push(Bool.False);
        }

        [Builtin("not", InArity = 1)]
        private static void Not(IContext context, AmStack stack)
        {
            stack.Push(stack.Pop<Bool>().Not());
        }

        [Builtin("and", InArity = 2)]
        private static void And(IContext context, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 && b2));
        }

        [Builtin("or", InArity = 2)]
        private static void Or(IContext context, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 || b2));
        }
    }
}
