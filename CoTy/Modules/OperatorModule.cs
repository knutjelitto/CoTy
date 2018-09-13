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

        [Builtin("==", InArity = 2, OutArity = 1)]
        private static void Equal(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Equals(value1, value2)));
        }

        [Builtin("!=", InArity = 2, OutArity = 1)]
        private static void NotEqual(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(!Equals(value1, value2)));
        }

        [Builtin("<", InArity = 2, OutArity = 1)]
        private static void LessThan(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Dyn.Compare(value1, value2) < 0));
        }

        [Builtin("<=", InArity = 2, OutArity = 1)]
        private static void LessThanOrEqual(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Dyn.Compare(value1, value2) <= 0));
        }

        [Builtin(">", InArity = 2, OutArity = 1)]
        private static void GreaterThan(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Dyn.Compare(value1, value2) > 0));
        }

        [Builtin(">=", InArity = 2, OutArity = 1)]
        private static void GreaterThanOrEqual(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Bool.From(Dyn.Compare(value1, value2) >= 0));
        }

        [Builtin("+", InArity = 2, OutArity = 1)]
        private static void Plus(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Dyn.Plus(value1, value2));
        }

        [Builtin("-", InArity = 2, OutArity = 1)]
        private static void Minus(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Dyn.Minus(value1, value2));
        }

        [Builtin("*", InArity = 2, OutArity = 1)]
        private static void Star(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Dyn.Star(value1, value2));
        }

        [Builtin("/", InArity = 2, OutArity = 1)]
        private static void Slash(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Dyn.Slash(value1, value2));
        }

        [Builtin("succ", InArity = 1, OutArity = 1)]
        private static void Succ(Context context, Stack stack)
        {
            var value = stack.Pop();

            var result = Dyn.Succ(value);

            stack.Push(result);
        }

        [Builtin("pred", InArity = 1, OutArity = 1)]
        private static void Pred(Context context, Stack stack)
        {
            var value = stack.Pop();

            var result = Dyn.Pred(value);

            stack.Push(result);
        }

        [Builtin("concat", "++", InArity = 2)]
        private static void Concat(Context context, Stack stack)
        {
            var seq2 = stack.Pop();
            var seq1 = stack.Pop();

            var result = Dyn.Concat(seq1, seq2);

            stack.Push(result);
        }
    }
}
