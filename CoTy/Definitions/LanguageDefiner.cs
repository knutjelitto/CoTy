using System;
using System.IO;
using System.Linq;

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
            Define(into, "eval", (scope, stack, value) => value.Eval(scope, stack));
            Define(into, "quote", (scope, stack, value) => Block.From(scope, Enumerable.Repeat(value, 1)));
            Define(into,
                   "if",
                   (scope, stack, condition, ifTrue, ifElse) =>
                   {
                       condition.Eval(scope, stack);
                       var result = stack.Pop();

                       if (result is bool boolean && boolean)
                       {
                           ifTrue.Eval(scope, stack);
                       }
                       else
                       {
                           ifElse.Eval(scope, stack);
                       }
                   });
            Define(into, "load", (scope, stack, symbol) => Load(scope, stack, GetSymbol(symbol)));
            Define(into, "read", (scope, stack, symbol) => Read(scope, GetSymbol(symbol)));
            Define(into, "merge", (scope, stack, block) => Merge(scope, stack, (dynamic)block));
        }

        public static void Merge(IScope scope, IStack stack, Block block)
        {
            var binder = block.Eval(stack);
            
            foreach (var symbol in binder.Symbols)
            {
                scope.Define(symbol, binder.Find(symbol).Value);
            }
        }

        public static Block Read(IScope scope, Symbol symbol)
        {
            var name = symbol.ToString().Replace('.', Path.DirectorySeparatorChar);

            var path = Path.Combine(Environment.CurrentDirectory, "Code", name);

            path = string.Concat(path, ".coty");

            var content = File.ReadAllText(path);

            var binder = Binder.From(symbol);
            var localScope = scope.Chain(binder);
            var localStack = Stack.Empty;

            return Read(localScope, localStack, content);
        }

        public static Block Read(IScope scope, IStack stack, string stream)
        {
            return Read(scope, stack, new CharStream(stream));
        }

        public static Block Read(IScope scope, IStack stack, ItemStream<char> charStream)
        {
            return Read(scope, stack, new Parser(charStream));
        }

        public static Block Read(IScope scope, IStack stack, ItemStream<object> itemStream)
        {
            try
            {
                return Block.From(scope, itemStream);
            }
            catch (CotyException cotyException)
            {
                G.C.WriteLine($"{cotyException.Message}");

                throw;
            }
        }

        public static void Load(IScope scope, IStack stack, Symbol symbol)
        {
            var block = Read(scope, symbol);
            Merge(scope, stack, block);
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
                        value.Eval(scope, stack);
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
