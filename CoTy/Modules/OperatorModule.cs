using CoTy.Ambiance;

namespace CoTy.Modules
{
    public class OperatorModule : Module
    {
        [Builtin("==", InArity = 2)]
        private static void Equal(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Equal(i2));
        }

        [Builtin("!=", InArity = 2)]
        private static void NotEqual(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.NotEqual(i2));
        }

        [Builtin("<", InArity = 2)]
        private static void Less(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Less(i2));
        }

        [Builtin("<=", InArity = 2)]
        private static void LessEqual(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.LessOrEqual(i2));
        }

        [Builtin(">", InArity = 2)]
        private static void Greater(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Greater(i2));
        }

        [Builtin(">=", InArity = 2)]
        private static void GreaterEqual(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.GreaterOrEqual(i2));
        }

        [Builtin("+", InArity = 2)]
        private static void Plus(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Add(i2));
        }

        [Builtin("-", InArity = 2)]
        private static void Minus(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Sub(i2));
        }

        [Builtin("*", InArity = 2)]
        private static void Multiply(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Mul(i2));
        }

        [Builtin("/", InArity = 2)]
        private static void Divide(IContext context, AmStack stack)
        {
            var i2 = (dynamic)stack.Pop();
            var i1 = (dynamic)stack.Pop();

            stack.Push(i1.Div(i2));
        }

        [Builtin("succ", InArity = 1)]
        private static void Succ(IContext context, AmStack stack)
        {
            var value = (dynamic)stack.Pop();

            stack.Push(value.Succ());
        }

        [Builtin("pred", InArity = 1)]
        private static void Pred(IContext context, AmStack stack)
        {
            var value = (dynamic)stack.Pop();

            stack.Push(value.Pred());
        }
    }
}
