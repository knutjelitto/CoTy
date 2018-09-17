using System;
using System.IO;

using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public class LanguageDefiner : Definer
    {
        public LanguageDefiner() : base("language") { }

        public override Context Define(Context @into)
        {
            Define(into, "apply", (context, stack, value) => value.Apply(context, stack));
            Define(into, "close", (context, stack, value) => value.Close(context, stack));
            Define(into, "quote", (context, stack, value) => Closure.From(context, value));
            Define(into, "unquote", 
                   (context, stack, values) =>
                   {
                       foreach (var value in Enumerate(values))
                       {
                           stack.Push(value);
                       }
                   });
            Define(into,
                   "if",
                   (context, stack, condition, ifTrue, ifElse) =>
                   {
                       condition.Apply(context, stack);
                       var result = stack.Pop();

                       if (result is bool boolean && boolean)
                       {
                           ifTrue.Apply(context, stack);
                       }
                       else
                       {
                           ifElse.Apply(context, stack);
                       }
                   });
            Define(into, "load", (context, stack, symbol) => Load(context, stack, GetSymbol(symbol)));

            return base.Define(into);
        }


        private static void Load(Context context, Stack stack, Symbol symbol)
        {
            var name = symbol.ToString();

            var path = Path.Combine(Environment.CurrentDirectory, "Modules", name);

            if (!Path.HasExtension(path))
            {
                path = Path.ChangeExtension(Path.Combine(Environment.CurrentDirectory, "Modules", symbol.ToString()), ".coty");
            }

            path = path.Replace("/", "\\");

            var content = File.ReadAllText(path);

            var localContext =  context.Push(name);
            var localStack = Stack.From();

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
