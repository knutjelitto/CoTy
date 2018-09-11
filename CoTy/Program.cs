using System;
using System.Collections.Generic;
using System.IO;

using CoTy.Ambiance;
using CoTy.Errors;
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
            var rootLexical = MakeRootFrame();
            var testLexical = WithTest(rootLexical);
            var rootActivation = rootLexical.Push("prompt");
            var testActivation = testLexical.Push("test");
            var stack = new AmStack();

            Execute(MakeParser(new StringStream(Read("tests"))), testActivation, stack);
            Execute(MakeParser(new StringStream(Read("startup"))), rootActivation, stack);
            while (true)
            {
                Execute(MakeParser(new ConsoleStream(stack.Dump)), rootActivation, stack);
            }
        }

        private static void Execute(IEnumerable<Cobject> stream, Context context, AmStack stack)
        {
            try
            {
                foreach (var value in stream)
                { 
                    try
                    {
                        value.Close(context, stack);
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
                    catch (TypeMismatchException typeEx)
                    {
                        Console.WriteLine($"{typeEx.Message}");
                    }
                }
            }
            catch (ScannerException scannerEx)
            {
                Console.WriteLine($"{scannerEx.Message}");
            }
            catch (ParserException parserEx)
            {
                Console.WriteLine($"{parserEx.Message}");
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

            return context;
        }

        private static Context WithTest(Context rootLexical)
        {
            var context = rootLexical.Push("testing");
            return Module.Reflect(typeof(TestingModule), context);
        }

        private static Parser MakeParser(ItemStream<char> input)
        {
            var source = new CharSource(input);
            var scanner = new Scanner(source);
            var parser = new Parser(scanner);

            return parser;
        }

        private static string Read(string name)
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
