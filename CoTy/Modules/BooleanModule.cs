using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class BooleanModule : Module
    {
        public BooleanModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("bool?")]
        private static void IsBool(AmScope scope, AmStack stack)
        {
            stack.Push(CoBoolean.From(stack.Pop() is CoBoolean));
        }

        [Builtin("true")]
        private static void True(AmScope scope, AmStack stack)
        {
            stack.Push(CoBoolean.True);
        }

        [Builtin("false")]
        private static void False(AmScope scope, AmStack stack)
        {
            stack.Push(CoBoolean.False);
        }
    }
}
