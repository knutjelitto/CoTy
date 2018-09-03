using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class OperatorModule : Module
    {
        public OperatorModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("+")]
        private static void Plus(AmScope scope, AmStack stack)
        {
            var i2 = stack.Pop();
            var i1 = stack.Pop();

            stack.Push(i1.Add(i2));
        }

        [Builtin("-")]
        private static void Minus(AmScope scope, AmStack stack)
        {
            var i2 = stack.Pop();
            var i1 = stack.Pop();

            stack.Push(i1.Sub(i2));
        }
    }
}
