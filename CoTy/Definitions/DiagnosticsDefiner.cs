using System;
using CoTy.Objects;
using CoTy.Support;

namespace CoTy.Definitions
{
    public class DiagnosticsDefiner : Definer
    {
        public DiagnosticsDefiner() : base("diagnostics") { }

        public override void Define(IScope into)
        {
            Define(into, "cx",
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
