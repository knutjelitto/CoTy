using System;
using System.IO;

using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class LanguageModule : Module
    {
        public LanguageModule(Context parent) : base(parent, "language")
        {
        }

        [Builtin("apply")]
        private static void DoApply(Context context, Stack stack)
        {
            var value = stack.Pop();

            Apply(context, stack, value);
        }

        [Builtin("close")]
        private static void DoClose(Context context, Stack stack)
        {
            var value = stack.Pop();

            Close(context, stack, value);
        }

        [Builtin("dequote")]
        private static void Dequote(Context context, Stack stack)
        {
            var value = stack.Pop();

            Dyn.Dequote(stack, value);
        }

        [Builtin("quote", InArity = 1)]
        private static void Quote(Context context, Stack stack)
        {
            var value = stack.Pop();

            var result = new Closure(context, value);

            stack.Push(result);
        }

        [Builtin("if")]
        private static void If(Context context, Stack stack)
        {
            var ifElse = stack.Pop();
            var ifTrue = stack.Pop();
            var condition = stack.Pop();

            Apply(context, stack, condition);
            var result = stack.Pop();

            if (result is Bool boolean && boolean.Value)
            {
                Apply(context, stack, ifTrue);
            }
            else
            {
                Apply(context, stack, ifElse);
            }
        }

        [Builtin("curry", IsOpaque = false)]  // a quot1 ⇒ quot2
        private static void Curry(Context context, Stack stack)
        {
            var quotation = stack.Pop();
            var value = stack.Pop();

            stack.Push(new Closure(context, quotation, value, Symbol.ApplySym));
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

            Execute(MakeParser(new StringStream(content)), localContext, localStack);

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
            Execute(new StringStream(stream), context, stack);
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
