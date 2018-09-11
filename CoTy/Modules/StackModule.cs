using CoTy.Ambiance;
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

        [Builtin("clear-stack", "cs")]
        private static void ClearStack(Context context, AmStack stack)
        {
            stack.Clear();
        }

        [Builtin("get-stack", "gs")]
        private static void GetStack(Context context, AmStack stack)
        {
            var quot = stack.Get();
            stack.Push(quot);
        }

        [Builtin("drop")]
        private static void Drop(Context context, AmStack stack)
        {
            stack.Pop();
        }

        [Builtin("dup")]
        private static void Dup(Context context, AmStack stack)
        {
            stack.Push(stack.Peek());
        }

        [Builtin("swap")]
        private static void Swap(Context context, AmStack stack)
        {
            var x2 = stack.Pop();
            var x1 = stack.Pop();

            stack.Push(x2);
            stack.Push(x1);
        }
    }
}
