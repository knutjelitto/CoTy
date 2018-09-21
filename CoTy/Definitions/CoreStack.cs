using CoTy.Objects;

namespace CoTy.Definitions
{
    public class CoreStack : Core
    {
        public CoreStack() : base("stack") { }

        public override void Define(Maker into)
        {
            into.Define("drop", (scope, stack) => stack.Drop());
            into.Define("dup", (scope, stack) => stack.Dup());
            into.Define("swap", (scope, stack) => stack.Swap());
            into.Define("over", (scope, stack) => stack.Over());
        }
    }
}
