using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class BooleanModule : Module
    {
        public BooleanModule(AmScope parent) : base(parent)
        {
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
