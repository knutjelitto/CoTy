using System;
using System.IO;

using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Objects;
using CoTy.Support;

namespace CoTy.Definitions
{
    public class LanguageDefiner : Definer
    {
        public LanguageDefiner() : base("language") { }

        public override void Define(IContext into)
        {
            Define(into, "apply", (context, stack, value) => value.Apply(context, stack));
            Define(into, "lambda", (context, stack, value) => value.Lambda(context, stack));
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
        }


        private static void Load(IContext context, IStack stack, Symbol symbol)
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

        public static void Execute(string stream, IContext context, IStack stack)
        {
            Execute(new CharStream(stream), context, stack);
        }

        public static void Execute(ItemStream<char> charStream, IContext context, IStack stack)
        {
            Execute(new Parser(charStream), context, stack);
        }

        public static void Execute(ItemStream<Cobject> stream, IContext context, IStack stack)
        {
            try
            {
                foreach (var value in stream)
                {
                    try
                    {
                        value.Lambda(context, stack);
                    }
                    catch (CotyException cotyException)
                    {
                        G.C.WriteLine($"{cotyException.Message}");
                    }
                }
            }
            catch (CotyException cotyException)
            {
                G.C.WriteLine($"{cotyException.Message}");
            }
        }
    }
}
