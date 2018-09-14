using System.IO;

using CoTy.Inputs;
using CoTy.Modules;
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
            var stack = new Stack();

            LanguageModule.Execute(ReadResource("tests"), testActivation, stack);
            LanguageModule.Execute(ReadResource("startup"), rootActivation, stack);
            while (true)
            {
                LanguageModule.Execute(new ConsoleStream(stack.Dump), rootActivation, stack);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static Context MakeRootFrame()
        {
            var modules = new Module[]
            {
                new LanguageModule(),
                new BindingModule(),
                new StackModule(),
                new BoolModule(),
                new OperatorModule(),
                new SequenceModule(),
                new SystemModule(),
                new ConsoleModule(),
                new DiagnosticsModule(),
            };
            var context = Context.Root("bottom");
            foreach (var module in modules)
            {
                context = module.Reflect(context.Push(module.Name));
            }

            return context;
        }

        private static Context WithTest(Context rootLexical)
        {
            return new TestingModule().Reflect(rootLexical.Push("testing"));
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
