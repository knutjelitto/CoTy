using System;

using CoTy.Objects;
using CoTy.Support;

namespace CoTy.Definitions
{
    public class ConsoleDefiner : Definer
    {
        public ConsoleDefiner() : base("console") { }

        public override void Define(IContext into)
        {
            Define(into, "print", value => G.C.Write($"{value}"));
            Define(into, "println", value => G.C.WriteLine($"{value}"));
            Define(into, "nl", () => G.C.WriteLine());
            Define(into, "cls", () => G.C.Clear());
        }
    }
}
