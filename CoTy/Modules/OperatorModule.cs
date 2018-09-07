using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class OperatorModule : Module
    {
        public OperatorModule(AmScope parent) : base(parent, "operator")
        {
        }

        [Builtin("==", InArity = 2)]
        private static void Equals(IContext context, AmStack stack)
        {
            // use object's equality
            var p = stack.Pop2();
            stack.Push((Bool)p.x.Equals(p.y));
        }

        [Builtin("!=", InArity = 2)]
        private static void NotEquals(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.NotEquals(p.y));
        }

        [Builtin("<", InArity = 2)]
        private static void Less(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Less(p.y));
        }

        [Builtin("<=", InArity = 2)]
        private static void LessOrEquals(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.LessOrEquals(p.y));
        }

        [Builtin(">", InArity = 2)]
        private static void Greater(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Greater(p.y));
        }

        [Builtin(">=", InArity = 2)]
        private static void GreaterOrdEquals(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.GreaterOrEquals(p.y));
        }

        [Builtin("+", InArity = 2)]
        private static void Plus(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Add(p.y));
        }

        [Builtin("-", InArity = 2)]
        private static void Minus(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Sub(p.y));
        }

        [Builtin("*", InArity = 2)]
        private static void Multiply(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Mul(p.y));
        }

        [Builtin("/", InArity = 2)]
        private static void Divide(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Div(p.y));
        }

        [Builtin("succ", InArity = 1)]
        private static void Succ(IContext context, AmStack stack)
        {
            var value = stack.Popd();
            stack.Push(value.Succ());
        }

        [Builtin("pred", InArity = 1)]
        private static void Pred(IContext context, AmStack stack)
        {
            var value = stack.Popd();
            stack.Push(value.Pred());
        }

        [Builtin("++", InArity = 2)]
        private static void Concatenate(IContext context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Concatenate(p.y));
        }
    }
}
