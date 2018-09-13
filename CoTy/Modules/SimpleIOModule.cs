using System;

using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class SimpleIOModule : Module
    {
        public SimpleIOModule(Context parent) : base(parent, "simple-io")
        {
        }

        [Builtin("print", InArity = 1)]
        private static void Print(Context context, Stack stack)
        {
            Dyn.Print(stack.Pop());
        }

        [Builtin("println", InArity = 1)]
        private static void Println(Context context, Stack stack)
        {
            Dyn.Println(stack.Pop());
        }

        [Builtin("newline", InArity = 0)]
        private static void Newline(Context context, Stack stack)
        {
            Console.WriteLine();
        }

        [Builtin("cls", InArity = 0)]
        private static void ClearScreen(Context context, Stack stack)
        {
            Console.Clear();
        }
    }
}
