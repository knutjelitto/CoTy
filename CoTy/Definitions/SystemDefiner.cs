using System;

using CoTy.Objects;

namespace CoTy.Definitions
{
    public class SystemDefiner : Definer
    {
        public SystemDefiner() : base("system") { }

        public override void Define(IContext into)
        {
            Define(into, "exit", () => Environment.Exit(42));
        }
    }
}
