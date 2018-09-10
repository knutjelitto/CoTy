using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class BoolModule : Module
    {
        public BoolModule(AmScope parent) : base(parent, "bool")
        {
        }

        [Builtin("bool?", InArity = 1)]
        private static void IsBool(AmScope context, AmStack stack)
        {
            stack.Push(Bool.From(stack.Pop() is Bool));
        }

        [Builtin("true", InArity = 0)]
        private static void True(AmScope context, AmStack stack)
        {
            stack.Push(Bool.True);
        }

        [Builtin("false", InArity = 0)]
        private static void False(AmScope context, AmStack stack)
        {
            stack.Push(Bool.False);
        }

        [Builtin("not", InArity = 1)]
        private static void Not(AmScope context, AmStack stack)
        {
            stack.Push(stack.Pop<Bool>().Not());
        }

        [Builtin("and", InArity = 2)]
        private static void And(AmScope context, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 && b2));
        }

        [Builtin("or", InArity = 2)]
        private static void Or(AmScope context, AmStack stack)
        {
            var b2 = stack.Pop<Bool>();
            var b1 = stack.Pop<Bool>();

            stack.Push(Bool.From(b1 || b2));
        }
    }
}
