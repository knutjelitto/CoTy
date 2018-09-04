using System.Collections.Generic;

using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Modules;
using System.Reflection;
using System.IO;
using CoTy.Objects;

namespace CoTy
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            var scope = MakeStandardScope();
            var stack = new AmStack();

            new Quotation(MakeParser(Read())).Eval(new TestModule(scope), stack);
            new Quotation(MakeParser(new ConsoleInput(stack.Dump))).Eval(scope, stack);
        }

        private static AmScope MakeStandardScope()
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            AmScope scope;
            scope = new LanguageModule(null);
            scope = new StackModule(scope);
            scope = new BooleanModule(scope);
            scope = new OperatorModule(scope);
            scope = new SequenceModule(scope);
            scope = new SystemModule(scope);

            return scope;
        }

        private static Parser MakeParser(IEnumerable<char> input)
        {
            var source = new CharSource(input);
            var scanner = new Scanner(source);
            var parser = new Parser(scanner);

            return parser;
        }

        private static string Read()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "CoTy.AAA.startup.coty";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
