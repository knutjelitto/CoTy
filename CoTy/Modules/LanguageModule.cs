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
            value.Apply(context, stack);
        }

        [Builtin("dequote")]
        private static void DeQuote(Context context, Stack stack)
        {
            foreach (var value in stack.Pop())
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

            condition.Apply(context, stack);
            var result = stack.Pop();

            if (result is Bool boolean && boolean.Value)
            {
                ifTrue.Apply(context, stack);
            }
            else
            {
                ifElse.Apply(context, stack);
            }
        }

        [Builtin("curry", IsOpaque = false)]  // a quot1 ⇒ quot2
        private static void Curry(Context context, Stack stack)
        {
            var p = stack.Pop2();

            stack.Push(new Closure(context, p.x, p.y, Symbol.ApplySym));
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
            var source = new CharSource(input);
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
                        value.Close(context, stack);
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
