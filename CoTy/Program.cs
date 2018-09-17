using System.IO;
using CoTy.Definitions;
using CoTy.Inputs;
using CoTy.Objects;

namespace CoTy
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
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

        private static Context MakeRootFrame()
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
            var context = Context.Root("bottom");
            foreach (var module in modules)
            {
                context = module.Define(context.Push(module.Name));
            }

            return context;
        }

        private static Context WithTest(Context rootLexical)
        {
            return new TestsDefiner().Define(rootLexical.Push("testing"));
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
