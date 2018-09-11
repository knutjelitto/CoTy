using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class BoolModule : Module
    {
        public BoolModule(Context parent) : base(parent, "bool")
        {
        }

        [Builtin("bool?", InArity = 1)]
        private static void IsBool(Context context, AmStack stack)
        {
            stack.Push(Bool.From(stack.Pop() is Bool));
        }

        [Builtin("true", InArity = 0)]
        private static void True(Context context, AmStack stack)
        {
            stack.Push(Bool.True);
        }

        [Builtin("false", InArity = 0)]
        private static void False(Context context, AmStack stack)
        {
            stack.Push(Bool.False);
        }

        [Builtin("not", InArity = 1)]
        private static void Not(Context context, AmStack stack)
        {
            stack.Push(stack.Pop<Bool>().Not());
        }

        [Builtin("and", InArity = 2)]
        private static void And(Context context, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 && b2));
        }

        [Builtin("or", InArity = 2)]
        private static void Or(Context context, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 || b2));
        }
    }
}
