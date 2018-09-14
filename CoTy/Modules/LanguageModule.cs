using System;
using System.IO;

using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Objects;
using CoTy.Objects.Implementations;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class LanguageModule : Module
    {
        public LanguageModule() : base("language") { }

        [Builtin("apply", InArity = 1)]
        private static void apply(Context context, Stack stack)
        {
            Impl.Apply(context, stack);
        }

        [Builtin("close", InArity = 1)]
        private static void close(Context context, Stack stack)
        {
            Impl.Close(context, stack);
        }

        [Builtin("quote", InArity = 1)]
        private static void quote(Context context, Stack stack)
        {
            Impl.Quote(context, stack);
        }

        [Builtin("unquote", InArity = 1)]
        private static void unquote(Context context, Stack stack)
        {
            Impl.Unquote(context, stack);
        }

        [Builtin("if")]
        private static void If(Context context, Stack stack)
        {
            Impl.If(context, stack);
        }

        [Builtin("curry", InArity = 2, OutArity = 1)]  // a quot1 ⇒ quot2
        private static void Curry(Context context, Stack stack)
        {
            Impl.Curry(context, stack);
        }

        [Builtin("load", InArity = 1)]
        private static void Load(Context context, Stack stack)
        {
            var symbol = GetSymbol(stack.Pop());
            var path = Path.Combine(Environment.CurrentDirectory, "Modules", symbol.ToString());

            if (!Path.HasExtension(path))
            {
                path = Path.ChangeExtension(Path.Combine(Environment.CurrentDirectory, "Modules", symbol.ToString()), ".coty");
            }

            path = path.Replace("/", "\\");

            var content = File.ReadAllText(path);

            var localContext =  context.Push("load");
            var localStack = new Stack();

            Execute(MakeParser(new CharStream(content)), localContext, localStack);

            localContext = localContext.Pop();

            context.Define(symbol, localContext);
        }

        public static Parser MakeParser(ItemStream<char> input)
        {
            var source = new ItemSource<char>(input);
            var scanner = new Scanner(source);
            var parser = new Parser(scanner);

            return parser;
        }

        public static void Execute(string stream, Context context, Stack stack)
        {
            Execute(new CharStream(stream), context, stack);
        }

        public static void Execute(ItemStream<char> charStream, Context context, Stack stack)
        {
            Execute(MakeParser(charStream), context, stack);
        }

        public static void Execute(ItemStream<Cobject> stream, Context context, Stack stack)
        {
            try
            {
                foreach (var value in stream)
                {
                    try
                    {
                        Close(context, stack, value);
                    }
                    catch (CotyException cotyException)
                    {
                        Console.WriteLine($"{cotyException.Message}");
                    }
                }
            }
            catch (CotyException cotyException)
            {
                Console.WriteLine($"{cotyException.Message}");
            }
        }
    }
}
