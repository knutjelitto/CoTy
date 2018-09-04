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
            void Dump(string what, AmFrame frame)
            {
                Console.WriteLine(what);
                while (frame != null)
                {
                    var syms = frame.Name + "{" + string.Join(" ", frame.Symbols) + "}";
                    Console.WriteLine(syms);
                    frame = frame.Parent;
                }
            }

            Dump("==activation==", scope.Activation);
            Dump("==lexical=====", scope.Lexical);
        }
    }
}
