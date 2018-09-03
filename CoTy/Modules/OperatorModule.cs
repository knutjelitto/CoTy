using CoTy.Ambiance;

namespace CoTy.Modules
{
    public class OperatorModule : Module
    {
        public OperatorModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("==")]
        private static void EQ(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.EQ(i2));
        }

        [Builtin("!=")]
        private static void NE(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.NE(i2));
        }

        [Builtin("<")]
        private static void LT(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.LT(i2));
        }

        [Builtin("<=")]
        private static void LE(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.LE(i2));
        }

        [Builtin(">")]
        private static void GT(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.GT(i2));
        }

        [Builtin(">=")]
        private static void GE(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.GE(i2));
        }

        [Builtin("+")]
        private static void Plus(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Add(i2));
        }

        [Builtin("-")]
        private static void Minus(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Sub(i2));
        }

        [Builtin("*")]
        private static void Multiply(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Mul(i2));
        }

        [Builtin("/")]
        private static void Divide(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Div(i2));
        }

        [Builtin("succ")]
        private static void Succ(AmScope scope, AmStack stack)
        {
            var value = (dynamic)stack.Pop();

            stack.Push(value.Succ());
        }

        [Builtin("pred")]
        private static void Pred(AmScope scope, AmStack stack)
        {
            var value = (dynamic)stack.Pop();

            stack.Push(value.Pred());
        }
    }
}
