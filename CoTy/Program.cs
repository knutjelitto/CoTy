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

            var rootLexical = MakeRootFrame(out var root);
            var testLexical = WithTest(root, rootLexical);
            var rootActivation = rootLexical.Chain(Binder.From("prompt"));
            var testActivation = testLexical.Chain(Binder.From("test"));
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

            CoWriter.Setup(0, 0, width, height);
        }

        private static IScope MakeRootFrame(out IBinder root)
        {
            var modules = new Definitions.Definer[]
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

            root = Binder.From("root");
            var scope = Context.Root(root);

            root.Define(root.Name, root);

            foreach (var module in modules)
            {
                var binder = Binder.From(module.Name);
                scope = scope.Chain(binder);
                root.Define(binder.Name, binder);

                module.Define(scope);
            }

            return scope;
        }

        private static IScope WithTest(IBinder root, IScope rootLexical)
        {
            var testing = Binder.From("testing");
            root.Define(testing.Name, testing);
            var withTest = rootLexical.Chain(testing);
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
