using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class OperatorModule : Module
    {
        public OperatorModule(Context parent) : base(parent, "operator")
        {
        }

        [Builtin("==", InArity = 2)]
        private static void Equals(Context context, AmStack stack)
        {
            // use object's equality
            var p = stack.Pop2();
            stack.Push((Bool)p.x.Equals(p.y));
        }

        [Builtin("!=", InArity = 2)]
        private static void NotEquals(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.NotEquals(p.y));
        }

        [Builtin("<", InArity = 2)]
        private static void Less(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Less(p.y));
        }

        [Builtin("<=", InArity = 2)]
        private static void LessOrEquals(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.LessOrEquals(p.y));
        }

        [Builtin(">", InArity = 2)]
        private static void Greater(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Greater(p.y));
        }

        [Builtin(">=", InArity = 2)]
        private static void GreaterOrdEquals(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.GreaterOrEquals(p.y));
        }

        [Builtin("+", InArity = 2)]
        private static void Plus(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Add(p.y));
        }

        [Builtin("-", InArity = 2)]
        private static void Minus(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Sub(p.y));
        }

        [Builtin("*", InArity = 2)]
        private static void Multiply(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Mul(p.y));
        }

        [Builtin("/", InArity = 2)]
        private static void Divide(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Div(p.y));
        }

        [Builtin("succ", InArity = 1)]
        private static void Succ(Context context, AmStack stack)
        {
            var value = stack.Popd();
            stack.Push(value.Succ());
        }

        [Builtin("pred", InArity = 1)]
        private static void Pred(Context context, AmStack stack)
        {
            var value = stack.Popd();
            stack.Push(value.Pred());
        }

        [Builtin("++", InArity = 2)]
        private static void Concatenate(Context context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Concatenate(p.y));
        }
    }
}
