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
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            var rootLexical = MakeRootFrame();
            var testLexical = WithTest(rootLexical);
            var rootActivation = new AmScope(rootLexical, "prompt");
            var testActivation = new AmScope(testLexical, "test");
            var stack = new AmStack();

            //Execute(MakeParser(Read("tests")).Parse(testLexical), testActivation, stack);
            Execute(MakeParser(Read("startup")).Parse(rootLexical), rootActivation, stack);
            Execute(MakeParser(new ConsoleInput(stack.Dump)).Parse(rootLexical), rootActivation, stack);
        }

        private static void Execute(IEnumerable<Cobject> stream, IContext context, AmStack stack)
        {
            foreach (var value in stream)
            {
                try
                {
                    value.Eval(context, stack);
                }
                catch (ScopeException scopeEx)
                {
                    Console.WriteLine($"{scopeEx.Message}");
                }
                catch (StackException stackEx)
                {
                    Console.WriteLine($"{stackEx.Message}");
                }
                catch (BinderException binderEx)
                {
                    Console.WriteLine($"{binderEx.Message}");
                }
            }
        }

        private static AmScope MakeRootFrame()
        {
            var root = new AmScope(null, "bottom");

            new LanguageModule().ImportInto(root);
            new BindingModule().ImportInto(root);
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
