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
        private static void Equals(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(((dynamic)value1).Compare(value1, value2) == 0));
        }

        [Builtin("!=", InArity = 2)]
        private static void NotEqual(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(((dynamic)value1).Compare(value1, value2) != 0));
        }

        [Builtin("<", InArity = 2)]
        private static void Less(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(((dynamic)value1).Compare(value1, value2) < 0));
        }

        [Builtin("<=", InArity = 2)]
        private static void LessOrEquals(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(((dynamic)value1).Compare(value1, value2) <= 0));
        }

        [Builtin(">", InArity = 2)]
        private static void Greater(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(((dynamic)value1).Compare(value1, value2) > 0));
        }

        [Builtin(">=", InArity = 2)]
        private static void GreaterOrEquals(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(((dynamic)value1).Compare(value1, value2) >= 0));
        }

        [Builtin("+", InArity = 2)]
        private static void Plus(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(((dynamic)value1).Add(value1, value2));
        }

        [Builtin("-", InArity = 2)]
        private static void Minus(Context context, Stack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Sub(p.y));
        }

        [Builtin("*", InArity = 2)]
        private static void Multiply(Context context, Stack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Mul(p.y));
        }

        [Builtin("/", InArity = 2)]
        private static void Divide(Context context, Stack stack)
        {
            var p = stack.Pop2d();
            stack.Push(p.x.Div(p.y));
        }

        [Builtin("succ", InArity = 1)]
        private static void Succ(Context context, Stack stack)
        {
            var value = stack.Popd();
            stack.Push(value.Succ());
        }

        [Builtin("pred", InArity = 1)]
        private static void Pred(Context context, Stack stack)
        {
            var value = stack.Popd();
            stack.Push(value.Pred());
        }

        [Builtin("concat", "++", InArity = 2)]
        private static void Concat(Context context, Stack stack)
        {
            var seq2 = stack.Pop();
            var seq1 = stack.Popd();

            stack.Push(seq1.Concat(seq1, seq2));
        }
    }
}
