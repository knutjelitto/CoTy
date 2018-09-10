using System;

using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class SimpleIOModule : Module
    {
        public SimpleIOModule(AmScope parent) : base(parent, "simple-io")
        {
        }

        [Builtin("print")]
        private static void Print(IContext context, AmStack stack)
        {
            var value = stack.Pop();

            Console.Write($"{value}");
        }

        [Builtin("printnl")]
        private static void PrintNl(IContext context, AmStack stack)
        {
            var value = stack.Pop();

            Console.WriteLine($"{value}");
        }

        [Builtin("newline")]
        private static void Newline(IContext context, AmStack stack)
        {
            Console.WriteLine();
        }
    }
}
