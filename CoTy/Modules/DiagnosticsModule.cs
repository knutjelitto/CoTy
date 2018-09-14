using System;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class DiagnosticsModule : Module
    {
        public DiagnosticsModule(Context parent) : base(parent, "diagnostics")
        {
        }

        [Builtin("context")]
        private static void DumpContext(Context context, Stack stack)
        {
            var scope = context.Scope;
            while (scope != null)
            {
                var syms = scope.Name + "{" + string.Join(" ", scope.Symbols) + "}";
                Console.WriteLine(syms);
                scope = scope.Parent;
            }
        }
    }
}
