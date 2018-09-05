using System;
using System.Threading;
using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class DiagnosticsModule : Module
    {
        [Builtin("ds")]
        private static void DumpScope(AmScope scope, AmStack stack)
        {
            while (scope != null)
            {
                var syms = scope.Name + "{" + string.Join(" ", scope.Symbols) + "}";
                Console.WriteLine(syms);
                scope = scope.Parent;
            }
        }
    }
}
