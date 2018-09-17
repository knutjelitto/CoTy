using System;

using CoTy.Objects;

namespace CoTy.Definitions
{
    public class ConsoleDefiner : Definer
    {
        public ConsoleDefiner() : base("console") { }

        public override Context Define(Context @into)
        {
            Define(into, "print", value => Console.Write($"{value}"));
            Define(into, "println", value => Console.WriteLine($"{value}"));
            Define(into, "nl", () => Console.WriteLine());
            Define(into, "cls", Console.Clear);

            return base.Define(@into);
        }
    }
}
