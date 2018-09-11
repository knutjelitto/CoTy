using System;
using System.Collections.Generic;
using System.IO;

using CoTy.Ambiance;
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
        private static void DoApply(Context context, AmStack stack)
        {
            var value = stack.Pop();
            value.Apply(context, stack);
        }

        [Builtin("dequote")]
        private static void DeQuote(Context context, AmStack stack)
        {
            foreach (var value in stack.Pop())
            {
                stack.Push(value);
            }
        }

        [Builtin("if")]
        private static void If(Context context, AmStack stack)
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
        private static void Curry(Context context, AmStack stack)
        {
            var p = stack.Pop2();

            stack.Push(new Closure(context, p.x, p.y, Symbol.ApplySym));
        }

        [Builtin("load", InArity = 1)]
        private static void Load(Context context, AmStack stack)
        {
            var symbol = GetSymbol(stack.Pop());
            var path = Path.Combine(Environment.CurrentDirectory, "Modules", symbol.ToString());

            if (!Path.HasExtension(path))
            {
                path = Path.ChangeExtension(Path.Combine(Environment.CurrentDirectory, "Modules", symbol.ToString()), ".coty");
            }

            var content = File.ReadAllText(path);

            var localContext =  context.Push("load");
            var localStack = new AmStack();

            Execute(MakeParser(new StringStream(content)), localContext, localStack);

            localContext = localContext.Pop();

            context.Define(symbol, localContext);
        }

        private static Parser MakeParser(ItemStream<char> input)
        {
            var source = new CharSource(input);
            var scanner = new Scanner(source);
            var parser = new Parser(scanner);

            return parser;
        }

        private static void Execute(IEnumerable<Cobject> stream, Context context, AmStack stack)
        {
            try
            {
                foreach (var value in stream)
                {
                    try
                    {
                        value.Close(context, stack);
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
            }
            catch (ScannerException scannerEx)
            {
                Console.WriteLine($"{scannerEx.Message}");
            }
            catch (ParserException parserEx)
            {
                Console.WriteLine($"{parserEx.Message}");
            }
        }



    }
}
