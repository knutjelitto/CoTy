using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Module : AmScope
    {
        protected Module(AmScope parent, string name) : base(parent, name)
        {
            Reflect(this);
        }

        public static void Load(AmScope context, AmStack stack, string defaultExtension = ".coty")
        {
            var symbol = GetSymbol(stack.Pop());
            var path = Path.Combine(Environment.CurrentDirectory, "Modules", symbol.ToString());

            if (!Path.HasExtension(path))
            {
                path = Path.ChangeExtension(Path.Combine(Environment.CurrentDirectory, "Modules", symbol.ToString()), defaultExtension);
            }

            var content = File.ReadAllText(path);

            var localContext = new AmScope((AmScope)context, "load-activation");
            var localStack = new AmStack();

            Execute(MakeParser(new StringStream(content)), localContext, localStack);

            localContext.Parent = null;
        }

        private static Parser MakeParser(ItemStream<char> input)
        {
            var source = new CharSource(input);
            var scanner = new Scanner(source);
            var parser = new Parser(scanner);

            return parser;
        }

        private static void Execute(IEnumerable<Cobject> stream, AmScope context, AmStack stack)
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


        protected static bool TryGetSymbol(Cobject candidate, out Symbol symbol)
        {
            if (candidate is Characters str)
            {
                symbol = Symbol.Get(str.Value);
            }
            else if (!(candidate is Closure quotation) || !quotation.TryGetQuotedSymbol(out symbol))
            {
                symbol = null;
                return false;
            }

            return true;
        }

        protected static Symbol GetSymbol(Cobject candidate)
        {
            if (!TryGetSymbol(candidate, out var symbol))
            {
                throw new BinderException($"`{candidate}´ can't be a symbol");
            }

            return symbol;
        }

        protected class BuiltinAttribute : Attribute
        {
            public BuiltinAttribute(string name, params string[] aliases)
            {
                Name = name;
                Aliases = aliases;
            }

            public string Name { get; }
            public string[] Aliases { get; }
            public int InArity { get; set; } = -1;
            public bool IsOpaque { get; set; } = true;
        }

        private void Reflect(AmScope goal)
        {
            foreach (var method in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                var info = method.GetCustomAttribute<BuiltinAttribute>();
                if (info != null)
                {
                    Action<AmScope, AmStack> eval;

                    var candidate = (Action<AmScope, AmStack>)Delegate.CreateDelegate(typeof(Action<AmScope, AmStack>), method, true);
                    if (info.InArity >= 0)
                    {
                        var arity = info.InArity;
                        var checkedEval = new Action<AmScope, AmStack>((context, stack) =>
                        {
                            if (stack.Count < arity)
                            {
                                throw new StackException(arity, stack.Count);
                            }
                            candidate(context, stack);
                        });
                        eval = checkedEval;
                    }
                    else
                    {
                        eval = candidate;
                    }
                    var builtin = new Builtin(eval);
                    goal.Define(Symbol.Get(info.Name), builtin, true, info.IsOpaque);
                    foreach (var alias in info.Aliases)
                    {
                        goal.Define(Symbol.Get(alias), builtin);
                    }
                }
            }
        }
    }
}
