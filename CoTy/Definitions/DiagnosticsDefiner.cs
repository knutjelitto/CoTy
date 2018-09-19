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
                       while (scope != null)
                       {
                           var syms = scope.Name + "{" + string.Join(" ", scope.Symbols) + "}";
                           G.C.WriteLine(syms);
                           scope = scope.Parent;
                       }
                   });
        }
    }
}
