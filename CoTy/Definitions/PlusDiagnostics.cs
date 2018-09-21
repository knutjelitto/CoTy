using CoTy.Objects;
using CoTy.Support;

namespace CoTy.Definitions
{
    public class PlusDiagnostics : Core
    {
        public PlusDiagnostics() : base("diagnostics") { }

        public override void Define(Maker into)
        {
            into.Define("cs", (scope, stack) => stack.Clear());
            into.Define("gs", (scope, stack) => stack.Get());
            into.Define(
                   "cx",
                   (scope, stack) =>
                   {
                       foreach (var binder in scope.Binders)
                       {
                           G.C.WriteLine($"{binder}");
                       }
                   });
        }
    }
}
