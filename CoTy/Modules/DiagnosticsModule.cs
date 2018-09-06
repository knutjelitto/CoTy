using System;
using System.Threading;
using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class DiagnosticsModule : Module
    {
        [Builtin("ds")]
        private static void DumpScope(IContext context, AmStack stack)
        {
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
        }
    }
}
