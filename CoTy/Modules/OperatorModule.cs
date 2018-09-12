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
        private static void Equal(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Equals(value1, value2)));
        }

        [Builtin("!=", InArity = 2)]
        private static void NotEqual(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(!Equals(value1, value2)));
        }

        [Builtin("<", InArity = 2)]
        private static void LessThan(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Eval.Compare(value1, value2) < 0));
        }

        [Builtin("<=", InArity = 2)]
        private static void LessThanOrEqual(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Eval.Compare(value1, value2) <= 0));
        }

        [Builtin(">", InArity = 2)]
        private static void GreaterThan(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Eval.Compare(value1, value2) > 0));
        }

        [Builtin(">=", InArity = 2)]
        private static void GreaterThanOrEqual(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Eval.Compare(value1, value2) >= 0));
        }

        [Builtin("+", InArity = 2)]
        private static void Plus(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Eval.Plus(value1, value2));
        }

        [Builtin("-", InArity = 2)]
        private static void Minus(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Eval.Minus(value1, value2));
        }

        [Builtin("*", InArity = 2)]
        private static void Star(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Eval.Star(value1, value2));
        }

        [Builtin("/", InArity = 2)]
        private static void Slash(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Eval.Slash(value1, value2));
        }

        [Builtin("succ", InArity = 1)]
        private static void Succ(Context context, Stack stack)
        {
            var value = stack.Pop();

            stack.Push(Eval.Succ(value));
        }

        [Builtin("pred", InArity = 1)]
        private static void Pred(Context context, Stack stack)
        {
            var value = stack.Pop();

            stack.Push(Eval.Pred(value));
        }

        [Builtin("concat", "++", InArity = 2)]
        private static void Concat(Context context, Stack stack)
        {
            var seq2 = stack.Pop();
            var seq1 = stack.Pop();

            stack.Push(Eval.Concat(seq1, seq2));
        }
    }
}
