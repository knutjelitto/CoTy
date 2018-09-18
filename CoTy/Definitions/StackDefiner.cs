using CoTy.Objects;

namespace CoTy.Definitions
{
    public class StackDefiner : Definer
    {
        public StackDefiner() : base("stack") { }

        public override void Define(IContext into)
        {
            Define(into, "cs", (context, stack) => stack.Clear());
            Define(into, "gs", (context, stack) => stack.Get());
            Define(into, "drop", (context, stack) => stack.Drop());
            Define(into, "dup", (context, stack) => stack.Dup());
            Define(into, "swap", (context, stack) => stack.Swap());
            Define(into, "over", (context, stack) => stack.Over());
        }
    }
}
