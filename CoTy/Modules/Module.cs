using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Inputs;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Module : Context
    {
        protected Module(Context parent, string name) : base(parent, name)
        {
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

        public static Context Reflect(Type moduleType, Context into)
        {
            var parts = Regex.Split(moduleType.Name, @"(\p{Lu}\p{Ll}*)").Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            foreach (var method in moduleType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                var info = method.GetCustomAttribute<BuiltinAttribute>();
                if (info != null)
                {
                    Action<Context, AmStack> eval;

                    var candidate = (Action<Context, AmStack>)Delegate.CreateDelegate(typeof(Action<Context, AmStack>), method, true);
                    if (info.InArity >= 0)
                    {
                        var arity = info.InArity;
                        var checkedEval = new Action<Context, AmStack>((context, stack) =>
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
                    into.Define(Symbol.Get(info.Name), builtin, true, info.IsOpaque);
                    foreach (var alias in info.Aliases)
                    {
                        into.Define(Symbol.Get(alias), builtin);
                    }
                }
            }

            return into;
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
    }
}
