using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class OperatorModule : Module
    {
        public OperatorModule() : base("operator") { }

        [Builtin("==", InArity = 2, OutArity = 1)]
        private static void Equal(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(Equals(value1, value2));
        }

        [Builtin("!=", InArity = 2, OutArity = 1)]
        private static void NotEqual(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            stack.Push(!Equals(value1, value2));
        }

        [Builtin("<", InArity = 2, OutArity = 1)]
        private static void LessThan(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1.CompareTo(value2) < 0;

            stack.Push(result);
        }

        [Builtin("<=", InArity = 2, OutArity = 1)]
        private static void LessThanOrEqual(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1.CompareTo(value2) <= 0;

            stack.Push(result);
        }

        [Builtin(">", InArity = 2, OutArity = 1)]
        private static void GreaterThan(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1.CompareTo(value2) > 0;

            stack.Push(result);
        }

        [Builtin(">=", InArity = 2, OutArity = 1)]
        private static void GreaterThanOrEqual(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1.CompareTo(value2) >= 0;

            stack.Push(result);
        }

        [Builtin("+", InArity = 2, OutArity = 1)]
        private static void Add(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1 + value2;

            stack.Push(result);
        }

        [Builtin("-", InArity = 2, OutArity = 1)]
        private static void Substract(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1 - value2;

            stack.Push(result);
        }

        [Builtin("*", InArity = 2, OutArity = 1)]
        private static void Multiply(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1 * value2;

            stack.Push(result);
        }

        [Builtin("/", InArity = 2, OutArity = 1)]
        private static void Divide(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1 / value2;

            stack.Push(result);
        }


        [Builtin("%", InArity = 2, OutArity = 1)]
        private static void Modulo(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1 % value2;

            stack.Push(result);
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
