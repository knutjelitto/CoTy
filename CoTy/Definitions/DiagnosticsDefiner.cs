using System;
using CoTy.Objects;
using CoTy.Support;

namespace CoTy.Definitions
{
    public class DiagnosticsDefiner : Definer
    {
        public DiagnosticsDefiner() : base("diagnostics") { }

        public override void Define(IContext into)
        {
            Define(into, "cx",
                   (context, stack) =>
                   {
                       var scope = context.Scope;
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
