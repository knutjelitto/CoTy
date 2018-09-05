using System;
using System.Collections.Generic;

using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Modules;
using System.Reflection;
using System.IO;
using CoTy.Objects;
using System.Linq;

namespace CoTy
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            var rootLexical = MakeRootFrame();
            var rootActivation = new AmScope(rootLexical, "activation");
            var stack = new AmStack();

            Execute(MakeParser(Read("startup")), new AmScope(WithTest(rootLexical), "test"), stack);
            Execute(MakeParser(new ConsoleInput(stack.Dump)), new AmScope(rootLexical, "lexical"), stack);
        }

        private static void Execute(IEnumerable<Cobject> stream, AmScope scope, AmStack stack)
        {
            foreach (var value in stream)
            {
                try
                {
                    var toEval = value;

                    if (value is QuotationLiteral quotationLiteral)
                    {
                        toEval = new Quotation(scope, quotationLiteral.ToList());
                    }
                    toEval.Eval(scope, stack);
                }
                catch (ScopeException scopeEx)
                {
                    Console.WriteLine($"{scopeEx.Message}");
                }
                catch (StackException stackEx)
                {
                    Console.WriteLine($"{stackEx.Message}");
                }
            }
        }

        private static AmScope MakeRootFrame()
        {
            var root = new AmScope(null, "root");

            new LanguageModule().ImportInto(root);
            new StackModule().ImportInto(root);
            new BoolModule().ImportInto(root);
            new OperatorModule().ImportInto(root);
            new SequenceModule().ImportInto(root);
            new SystemModule().ImportInto(root);
            new DiagnosticsModule().ImportInto(root);

            return root;
        }

        private static AmScope WithTest(AmScope rootLexical)
        {
            var withTest = new AmScope(rootLexical, "with-test");

            new TestModule().ImportInto(withTest);

            return withTest;
        }

        private static Parser MakeParser(IEnumerable<char> input)
        {
            var source = new CharSource(input);
            var scanner = new Scanner(source);
            var parser = new Parser(scanner);

            return parser;
        }

        private static string Read(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"CoTy.AAA.{name}.coty";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
