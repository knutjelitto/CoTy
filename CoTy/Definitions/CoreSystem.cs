using System;

using CoTy.Objects;

namespace CoTy.Definitions
{
    public class CoreSystem : Core
    {
        public CoreSystem() : base("system") { }

        public override void Define(Maker into)
        {
            into.Define("exit", () => Environment.Exit(42));
        }
    }
}
