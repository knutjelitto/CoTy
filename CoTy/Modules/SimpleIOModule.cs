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
            stack.Popd().Print();
        }

        [Builtin("println", InArity = 1)]
        private static void Println(Context context, Stack stack)
        {
            stack.Popd().Println();
        }

        [Builtin("newline", InArity = 0)]
        private static void Newline(Context context, Stack stack)
        {
            Console.WriteLine();
        }
    }
}
