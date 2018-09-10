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
        private static void Equals(AmScope context, AmStack stack)
        {
            // use object's equality
            var p = stack.Pop2();
            stack.Push((Bool)p.x.Equals(p.y));
        }

        [Builtin("!=", InArity = 2)]
        private static void NotEquals(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.NotEquals(p.y));
        }

        [Builtin("<", InArity = 2)]
        private static void Less(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Less(p.y));
        }

        [Builtin("<=", InArity = 2)]
        private static void LessOrEquals(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.LessOrEquals(p.y));
        }

        [Builtin(">", InArity = 2)]
        private static void Greater(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Greater(p.y));
        }

        [Builtin(">=", InArity = 2)]
        private static void GreaterOrdEquals(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.GreaterOrEquals(p.y));
        }

        [Builtin("+", InArity = 2)]
        private static void Plus(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Add(p.y));
        }

        [Builtin("-", InArity = 2)]
        private static void Minus(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Sub(p.y));
        }

        [Builtin("*", InArity = 2)]
        private static void Multiply(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Mul(p.y));
        }

        [Builtin("/", InArity = 2)]
        private static void Divide(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Div(p.y));
        }

        [Builtin("succ", InArity = 1)]
        private static void Succ(AmScope context, AmStack stack)
        {
            var value = stack.Popd();
            stack.Push(value.Succ());
        }

        [Builtin("pred", InArity = 1)]
        private static void Pred(AmScope context, AmStack stack)
        {
            var value = stack.Popd();
            stack.Push(value.Pred());
        }

        [Builtin("++", InArity = 2)]
        private static void Concatenate(AmScope context, AmStack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Concatenate(p.y));
        }
    }
}
