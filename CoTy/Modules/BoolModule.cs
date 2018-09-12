using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class BoolModule : Module
    {
        public BoolModule(Context parent) : base(parent, "bool")
        {
        }

        [Builtin("bool?", InArity = 1)]
        private static void IsBool(Context context, Stack stack)
        {
            stack.Push(Bool.From(stack.Pop() is Bool));
        }

        [Builtin("true", InArity = 0)]
        private static void True(Context context, Stack stack)
        {
            stack.Push(Bool.True);
        }

        [Builtin("false", InArity = 0)]
        private static void False(Context context, Stack stack)
        {
            stack.Push(Bool.False);
        }

        [Builtin("not", InArity = 1, OutArity = 1)]
        private static void Not(Context context, Stack stack)
        {
            var value = stack.Pop();

            var result = Eval.Not(value);

            stack.Push(result);
        }

        [Builtin("and", InArity = 2, OutArity = 1)]
        private static void And(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            var result = Eval.And(value1, value2);

            stack.Push(result);
        }

        [Builtin("or", InArity = 2, OutArity = 1)]
        private static void Or(Context context, Stack stack)
        {
            var value2 = stack.Pop();
            var value1 = stack.Pop();

            var result = Eval.Or(value1, value2);

            stack.Push(result);
        }
    }
}
