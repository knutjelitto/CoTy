using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class StackModule : Module
    {
        public StackModule(Context parent) : base(parent, "stack")
        {
        }

        [Builtin("clear-stack", "cs", InArity = 0)]
        private static void ClearStack(Context context, Stack stack)
        {
            stack.Clear();
        }

        [Builtin("get-stack", "gs", InArity = 0)]
        private static void GetStack(Context context, Stack stack)
        {
            var quot = stack.Get();
            stack.Push(quot);
        }

        [Builtin("drop", InArity = 1)]
        private static void Drop(Context context, Stack stack)
        {
            stack.Pop();
        }

        [Builtin("dup", InArity = 1)]
        private static void Dup(Context context, Stack stack)
        {
            stack.Push(stack.Peek());
        }

        [Builtin("swap", InArity = 2)]
        private static void Swap(Context context, Stack stack)
        {
            var x2 = stack.Pop();
            var x1 = stack.Pop();

            stack.Push(x2);
            stack.Push(x1);
        }
    }
}
