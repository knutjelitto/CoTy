using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class StackModule : Module
    {
        [Builtin("clear-stack", "cs")]
        private static void ClearStack(AmScope scope, AmStack stack)
        {
            stack.Clear();
        }

        [Builtin("drop")]
        private static void Drop(AmScope scope, AmStack stack)
        {
            stack.Pop();
        }

        [Builtin("dup")]
        private static void Dup(AmScope scope, AmStack stack)
        {
            stack.Push(stack.Peek());
        }

        [Builtin("swap")]
        private static void Swap(AmScope scope, AmStack stack)
        {
            var x2 = stack.Pop();
            var x1 = stack.Pop();

            stack.Push(x2);
            stack.Push(x1);
        }
    }
}
