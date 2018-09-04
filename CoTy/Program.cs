using System;
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
            var rootActivation = new AmFrame(null, "activation");
            var rootLexical = MakeRootFrame();
            var stack = new AmStack();

            Execute(MakeParser(Read("startup")), new AmScope(rootActivation, WithTest(rootLexical)), stack);
            Execute(MakeParser(new ConsoleInput(stack.Dump)), new AmScope(rootActivation,  rootLexical), stack);
        }

        private static void Execute(IEnumerable<Cobject> stream, AmScope scope, AmStack stack)
        {
            foreach (var value in stream)
            {
                try
                {
                    value.Eval(scope, stack);
                }
                catch (ScopeException scopeEx)
                {
                    Console.WriteLine($"{scopeEx.Message}");
                }
            }
        }

        private static AmFrame MakeRootFrame()
        {
            var root = new AmFrame(null, "root");

            new LanguageModule().ImportInto(root);
            new StackModule().ImportInto(root);
            new BoolModule().ImportInto(root);
            new OperatorModule().ImportInto(root);
            new SequenceModule().ImportInto(root);
            new SystemModule().ImportInto(root);
            new DiagnosticsModule().ImportInto(root);

            return root;
        }

        private static AmFrame WithTest(AmFrame rootLexical)
        {
            var withTest = new AmFrame(rootLexical, "with-test");

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
