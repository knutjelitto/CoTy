using System;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public class DiagnosticsDefiner : Definer
    {
        public DiagnosticsDefiner() : base("diagnostics") { }

        public override Context Define(Context @into)
        {
            Define(into, "ctx",
                   (context, stack) =>
                   {
                       var scope = context.Scope;
                       while (scope != null)
                       {
                           var syms = scope.Name + "{" + string.Join(" ", scope.Symbols) + "}";
                           Console.WriteLine(syms);
                           scope = scope.Parent;
                       }
                   });

            return base.Define(@into);
        }
    }
}
