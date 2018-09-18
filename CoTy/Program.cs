using System;
using System.IO;
using CoTy.Definitions;
using CoTy.Inputs;
using CoTy.Objects;
using CoTy.Support;

namespace CoTy
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            SetupConsole();

            Woo.Doo();

            var rootLexical = MakeRootFrame();
            var testLexical = WithTest(rootLexical);
            var rootActivation = rootLexical.Push("prompt");
            var testActivation = testLexical.Push("test");
            var stack = Stack.From();

            LanguageDefiner.Execute(ReadResource("tests"), testActivation, stack);
            LanguageDefiner.Execute(ReadResource("startup"), rootActivation, stack);
            while (true)
            {
                LanguageDefiner.Execute(new ConsoleStream(stack.Dump), rootActivation, stack);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void SetupConsole()
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;

            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);

            G.C = new CoWriter(10, 0, width - 10, height);
        }

        private static IContext MakeRootFrame()
        {
            var modules = new Definer[]
            {
                new LanguageDefiner(),
                new BindingDefiner(),
                new StackDefiner(),
                new OperatorDefiner(),
                new SequenceDefiner(),
                new SystemDefiner(),
                new ConsoleDefiner(),
                new DiagnosticsDefiner(),
            };

            var root = Context.Root("root");

            root.Define("root", root);

            var context = root;
            foreach (var module in modules)
            {
                context = context.Push(module.Name);
                root.Define(context.Name, context);

                module.Define(context);
            }

            return context;
        }

        private static IContext WithTest(IContext rootLexical)
        {
            var withTest = rootLexical.Push("testing");
            new TestsDefiner().Define(withTest);
            return withTest;
        }

        private static string ReadResource(string name)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceName = $"CoTy.Code.{name}.coty";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
