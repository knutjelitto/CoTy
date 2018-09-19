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

        public override void Define(IScope into)
        {
            Define(into, "apply", (scope, stack, value) => value.Apply(scope, stack));
            Define(into, "lambda", (scope, stack, value) => value.Lambda(scope, stack));
            Define(into, "quote", (scope, stack, value) => Closure.From(scope, value));
            Define(into, "flatten", 
                   (IScope scope, IStack stack, dynamic values) =>
                   {
                       foreach (var value in values)
                       {
                           stack.Push(value);
                       }
                   });
            Define(into,
                   "if",
                   (scope, stack, condition, ifTrue, ifElse) =>
                   {
                       condition.Apply(scope, stack);
                       var result = stack.Pop();

                       if (result is bool boolean && boolean)
                       {
                           ifTrue.Apply(scope, stack);
                       }
                       else
                       {
                           ifElse.Apply(scope, stack);
                       }
                   });
            Define(into, "load", (scope, stack, symbol) => Load(scope, stack, GetSymbol(symbol)));
        }


        private static void Load(IScope scope, IStack stack, Symbol symbol)
        {
            var name = symbol.ToString();

            var path = Path.Combine(Environment.CurrentDirectory, "Modules", name);

            if (!Path.HasExtension(path))
            {
                path = Path.ChangeExtension(Path.Combine(Environment.CurrentDirectory, "Modules", symbol.ToString()), ".coty");
            }

            path = path.Replace("/", "\\");

            var content = File.ReadAllText(path);

            var binder = Binder.From(symbol);
            var localScope =  scope.Chain(binder);
            var localStack = Stack.From();

            Execute(content, localScope, localStack);

            scope.Define(symbol, binder);
        }

        public static void Execute(string stream, IScope scope, IStack stack)
        {
            Execute(new CharStream(stream), scope, stack);
        }

        public static void Execute(ItemStream<char> charStream, IScope scope, IStack stack)
        {
            Execute(new Parser(charStream), scope, stack);
        }

        public static void Execute(ItemStream<object> stream, IScope scope, IStack stack)
        {
            try
            {
                foreach (var value in stream)
                {
                    try
                    {
                        value.Lambda(scope, stack);
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
