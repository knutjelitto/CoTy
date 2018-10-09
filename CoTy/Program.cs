using System;
using System.IO;
using CoTy.Definitions;
using CoTy.Errors;
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

            var rootBase = MakeRootFrame(out var root);
            var testBase = WithTest(root, rootBase);
            var rootScope = rootBase.Chain(Binder.From("prompt"));
            var testScope = testBase.Chain(Binder.From("test"));
            var stack = Stack.From();

            Capsuled(() =>  CoreLanguage.Load(testScope, stack, Symbol.Get("tests.all")));
            Capsuled(() => CoreLanguage.Load(rootScope, stack, Symbol.Get("startup")));

            Repl(rootScope, stack);
            // ReSharper disable once FunctionNeverReturns
        }

        private static void Repl(IScope scope, IStack stack)
        {
            while (true)
            {
                stack.Dump();
                var line = G.C.GetLine(":");
                Capsuled(
                    () =>
                    {
                        CoreLanguage.Execute(new CharStream(line), scope, stack);
                    });
            }
        }


        private static void Capsuled(Action action)
        {
            try
            {
                action();
            }
            catch (CotyException cotyException)
            {
                G.C.WriteLine($"{cotyException.Message}");
            }
            catch (Exception anyException)
            {
                G.C.WriteLine($"{anyException.Message}");
            }
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

        private static IScope MakeRootFrame(out Binder root)
        {
            var modules = new Core[]
            {
                new CoreLanguage(),
                new CoreBinder(),
                new CoreStack(),
                new CoreOperator(),
                new CoreSequence(),
                new CoreSystem(),
                new PlusConsole(),
                new PlusDiagnostics(),
            };

            root = Binder.From("root");
            var scope = Context.Root(root);

            root.Define(root.Name, root);

            foreach (var module in modules)
            {
                var binder = Binder.From(module.Name);
                scope = scope.Chain(binder);
                root.Define(binder.Name, binder);

                module.Define(new Definitions.Maker(scope));;
            }

            return scope;
        }

        private static IScope WithTest(IBinder root, IScope rootLexical)
        {
            var testing = Binder.From("testing");
            root.Define(testing.Name, testing);
            var withTest = rootLexical.Chain(testing);
            new PlusTest().Define(new Definitions.Maker(withTest));
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
