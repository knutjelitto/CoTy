﻿using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class StackModule : Module
    {
        public StackModule(AmScope parent) : base(parent, "stack")
        {
        }

        [Builtin("clear-stack", "cs")]
        private static void ClearStack(IContext context, AmStack stack)
        {
            stack.Clear();
        }

        [Builtin("drop")]
        private static void Drop(IContext context, AmStack stack)
        {
            stack.Pop();
        }

        [Builtin("dup")]
        private static void Dup(IContext context, AmStack stack)
        {
            stack.Push(stack.Peek());
        }

        [Builtin("swap")]
        private static void Swap(IContext context, AmStack stack)
        {
            var x2 = stack.Pop();
            var x1 = stack.Pop();

            stack.Push(x2);
            stack.Push(x1);
        }
    }
}