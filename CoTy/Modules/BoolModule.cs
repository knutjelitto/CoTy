using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class BoolModule : Module
    {
        public BoolModule() : base("bool") { }

        [Builtin("bool?", InArity = 1)]
        private static void IsBool(Context context, Stack stack)
        {
            stack.Push(stack.Pop() is bool);
        }

        [Builtin("true", InArity = 0)]
        private static void True(Context context, Stack stack)
        {
            stack.Push(true);
        }

        [Builtin("false", InArity = 0)]
        private static void False(Context context, Stack stack)
        {
            stack.Push(false);
        }

        [Builtin("!", InArity = 1, OutArity = 1)]
        private static void Not(Context context, Stack stack)
        {
            dynamic value = stack.Pop();

            var result = !value;

            stack.Push(result);
        }

        [Builtin("&", InArity = 2, OutArity = 1)]
        private static void And(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1 & value2;

            stack.Push(result);
        }

        [Builtin("|", InArity = 2, OutArity = 1)]
        private static void Or(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1 | value2;

            stack.Push(result);
        }


        [Builtin("^", InArity = 2, OutArity = 1)]
        private static void Xor(Context context, Stack stack)
        {
            dynamic value2 = stack.Pop();
            dynamic value1 = stack.Pop();

            var result = value1 ^ value2;

            stack.Push(result);
        }
    }
}
