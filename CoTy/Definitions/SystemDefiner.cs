using System;

using CoTy.Objects;

namespace CoTy.Definitions
{
    public class SystemDefiner : Definer
    {
        public SystemDefiner() : base("system") { }

        public override Context Define(Context into)
        {
            Define(into, "exit", () => Environment.Exit(42));

            return base.Define(into);
        }
    }
}
