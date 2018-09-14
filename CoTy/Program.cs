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

            LanguageModule.Execute(ReadResource("startup"), rootActivation, stack);
            LanguageModule.Execute(ReadResource("tests"), testActivation, stack);
            while (true)
            {
                LanguageModule.Execute(new ConsoleStream(stack.Dump), rootActivation, stack);
            }
        }

        private static Context MakeRootFrame()
        {
            var context = Context.Root("language");
            context = Module.Reflect(typeof(LanguageModule), context);
            context = Module.Reflect(typeof(BindingModule), context.Push("binding"));
            context = Module.Reflect(typeof(StackModule), context.Push("stack"));
            context = Module.Reflect(typeof(BoolModule), context.Push("bool"));
            context = Module.Reflect(typeof(OperatorModule), context.Push("operator"));
            context = Module.Reflect(typeof(SequenceModule), context.Push("sequence"));
            context = Module.Reflect(typeof(SystemModule), context.Push("system"));
            context = Module.Reflect(typeof(SimpleIOModule), context.Push("simple-io"));
            context = Module.Reflect(typeof(DiagnosticsModule), context.Push("diagnostics"));
            context = Module.Reflect(typeof(ShellModule), context.Push("shell"));

            return context;
        }

        private static Context WithTest(Context rootLexical)
        {
            var context = rootLexical.Push("testing");
            return Module.Reflect(typeof(TestingModule), context);
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
