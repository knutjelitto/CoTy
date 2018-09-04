using CoTy.Ambiance;

namespace CoTy.Modules
{
    public class OperatorModule : Module
    {
        public OperatorModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("==")]
        private static void Equal(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Equal(i2));
        }

        [Builtin("!=")]
        private static void NotEqual(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.NotEqual(i2));
        }

        [Builtin("<")]
        private static void Less(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Less(i2));
        }

        [Builtin("<=")]
        private static void LessEqual(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.LessOrEqual(i2));
        }

        [Builtin(">")]
        private static void Greater(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Greater(i2));
        }

        [Builtin(">=")]
        private static void GreaterEqual(AmScope scope, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.GreaterOrEqual(i2));
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
