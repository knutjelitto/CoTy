using System;
using System.Reflection;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Module : Cobject
    {
        protected Module(string name)
        {
            Name = name;
        }

        public string Name { get; }

        protected static bool TryGetSymbol(object candidate, out Symbol symbol)
        {
            if (candidate is Characters str)
            {
                symbol = Symbol.Get(str.Value);
            }
            else if (!(candidate is Sequence sequence) || !sequence.TryGetQuotedSymbol(out symbol))
            {
                symbol = null;
                return false;
            }

            return true;
        }

        protected static Symbol GetSymbol(object candidate)
        {
            if (!TryGetSymbol(candidate, out var symbol))
            {
                throw new BinderException($"`{string.Join(" ", Enumerate(candidate))}´ can't be a symbol");
            }

            return symbol;
        }

        public virtual Context Reflect(Context into)
        {
            Reflect(GetType(), into);
            return into;
        }

        private static void Reflect(Type moduleType, Context into)
        {
            foreach (var method in moduleType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                var info = method.GetCustomAttribute<BuiltinAttribute>();
                if (info != null)
                {
                    Action<Context, Stack> eval;

                    var candidate = (Action<Context, Stack>)Delegate.CreateDelegate(typeof(Action<Context, Stack>), method, true);
                    if (info.InArity >= 0)
                    {
                        var arity = info.InArity;
                        var checkedEval = new Action<Context, Stack>((context, stack) =>
                        {
                            if (stack.Value.Count < arity)
                            {
                                throw new StackException(arity, stack.Value.Count);
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
            public int OutArity { get; set; } = -1;
            public bool IsOpaque { get; set; } = true;
        }
    }
}
