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

            Execute(MakeParser(Read("tests")), testActivation, stack);
            Execute(MakeParser(Read("startup")), rootActivation, stack);
            while (true)
            {
                Execute(MakeParser(new ConsoleInput(stack.Dump)), rootActivation, stack);
            }
        }

        private static bool Execute(IEnumerable<Cobject> stream, IContext context, AmStack stack)
        {
            try
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
                    catch (TypeMismatchException typeEx)
                    {
                        Console.WriteLine($"{typeEx.Message}");
                    }
                }

                return true;
            }
            catch (ScannerException scannerEx)
            {
                Console.WriteLine($"{scannerEx.Message}");
            }
            catch (ParserException parserEx)
            {
                Console.WriteLine($"{parserEx.Message}");
            }

            return false;
        }

        private static AmScope MakeRootFrame()
        {
            AmScope root = new LanguageModule(null);
            root = new BindingModule(root);
            root = new StackModule(root);
            root = new BoolModule(root);
            root = new OperatorModule(root);
            root = new SequenceModule(root);
            root = new SystemModule(root);
            root = new DiagnosticsModule(root);

            return root;
        }

        private static AmScope WithTest(AmScope rootLexical)
        {
            return new TestModule(rootLexical);
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
            var resourceName = $"CoTy.Code.{name}.coty";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
