using System;
using Microsoft.CSharp.RuntimeBinder;

using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Modules;
using System.Reflection;
using System.IO;

namespace CoTy
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = Read();
            var console = new ConsoleSource();
            var source = new CharSource(console);
            var scanner = new Scanner(source);
            var parser = new Parser(scanner);

            AmScope scope = null;
            scope = new LanguageModule(scope);
            scope = new StackModule(scope);
            scope = new BooleanModule(scope);
            scope = new OperatorModule(scope);
            scope = new SystemModule(scope);
            var stack = new AmStack();

            foreach (var @object in parser)
            {
                try
                {
                    @object.Apply(scope, stack);
                }
                catch (ScopeException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
                catch (RuntimeBinderException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

                stack.Dump();
            }
        }

        private static string Read()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "CoTy.AAA.Check.coty";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
