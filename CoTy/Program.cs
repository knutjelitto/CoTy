using System;
using CoTy.Ambiance;
using CoTy.Inputs;
using CoTy.Modules;

namespace CoTy
{
    class Program
    {
        static void Main(string[] args)
        {
            var console = new ConsoleSource();
            var source = new CharSource(console);
            var scanner = new Scanner(source);
            var parser = new Parser(scanner);

            AmScope scope = null;
            scope = new BooleanModule(scope);
            scope = new IntegerModule(scope);
            var stack = new AmStack();

            foreach (var @object in parser)
            {
                @object.Apply(scope, stack);
                stack.Dump();
            }
        }
    }
}
