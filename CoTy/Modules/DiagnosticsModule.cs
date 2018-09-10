using System;
using System.Threading;
using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class DiagnosticsModule : Module
    {
        public DiagnosticsModule(AmScope parent) : base(parent, "diagnostics")
        {
        }

        [Builtin("ds")]
        private static void DumpScope(AmScope context, AmStack stack)
        {
#if true
            var scope = context.Scope;
            while (scope != null)
            {
                var syms = scope.Name + "{" + string.Join(" ", scope.Symbols) + "}";
                Console.WriteLine(syms);
                scope = (AmScope)scope.Parent;
            }
#else
            Console.WriteLine("===lexical===");
            var lexical = context.Lexical;
            while (lexical != null)
            {
                var syms = lexical.Name + "{" + string.Join(" ", lexical.Symbols) + "}";
                Console.WriteLine(syms);
                lexical = lexical.Parent;
            }
            Console.WriteLine("===local=====");
            var local = context.Local;
            while (local != null)
            {
                var syms = local.Name + "{" + string.Join(" ", local.Symbols) + "}";
                Console.WriteLine(syms);
                local = local.Parent;
            }
#endif
        }
    }
}
