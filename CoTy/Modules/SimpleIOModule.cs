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
        private static void Print(AmScope context, AmStack stack)
        {
            stack.Popd().Print();
        }

        [Builtin("println")]
        private static void Println(AmScope context, AmStack stack)
        {
            stack.Popd().Println();
        }

        [Builtin("newline")]
        private static void Newline(AmScope context, AmStack stack)
        {
            Console.WriteLine();
        }
    }
}
