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
        public LanguageModule() : base("language") { }

        [Builtin("apply", InArity = 1)]
        private static void apply(Context context, Stack stack)
        {
            var value = stack.Pop();

            Apply(context, stack, value);
        }

        [Builtin("close", InArity = 1)]
        private static void close(Context context, Stack stack)
        {
            var value = stack.Pop();

            Close(context, stack, value);
        }

        [Builtin("quote", InArity = 1)]
        private static void quote(Context context, Stack stack)
        {
            var value = stack.Pop();

            var result = Closure.From(context, value);

            stack.Push(result);
        }

        [Builtin("unquote", InArity = 1)]
        private static void unquote(Context context, Stack stack)
        {
            var sequence = stack.Pop();

            foreach (var value in Enumerate(sequence))
            {
                stack.Push(value);
            }
        }

        [Builtin("if")]
        private static void If(Context context, Stack stack)
        {
            var ifElse = stack.Pop();
            var ifTrue = stack.Pop();
            var condition = stack.Pop();

            Apply(context, stack, condition);
            var result = stack.Pop();

            if (result is bool boolean && boolean)
            {
                Apply(context, stack, ifTrue);
            }
            else
            {
                Apply(context, stack, ifElse);
            }
        }

        [Builtin("curry", InArity = 2, OutArity = 1)]  // a quot1 ⇒ quot2
        private static void Curry(Context context, Stack stack)
        {
            var quotation = stack.Pop();
            var value = stack.Pop();

            stack.Push(Closure.From(context, quotation, value, Symbol.ApplySym));
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

            Execute(content, localContext, localStack);

            localContext = localContext.Pop();

            context.Define(symbol, localContext);
        }

        public static void Execute(string stream, Context context, Stack stack)
        {
            Execute(new CharStream(stream), context, stack);
        }

        public static void Execute(ItemStream<char> charStream, Context context, Stack stack)
        {
            Execute(new Parser(charStream), context, stack);
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
