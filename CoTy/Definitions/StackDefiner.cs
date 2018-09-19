using CoTy.Objects;

namespace CoTy.Definitions
{
    public class StackDefiner : Definer
    {
        public StackDefiner() : base("stack") { }

        public override void Define(IScope into)
        {
            Define(into, "cs", (scope, stack) => stack.Clear());
            Define(into, "gs", (scope, stack) => stack.Get());
            Define(into, "drop", (scope, stack) => stack.Drop());
            Define(into, "dup", (scope, stack) => stack.Dup());
            Define(into, "swap", (scope, stack) => stack.Swap());
            Define(into, "over", (scope, stack) => stack.Over());
        }
    }
}
