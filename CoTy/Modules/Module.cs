using System;
using System.Reflection;

using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Module
    {
        public void ImportInto(AmScope scope)
        {
            Reflect(scope);
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
        }

        private void Reflect(AmScope goal)
        {
            foreach (var method in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                var info = method.GetCustomAttribute<BuiltinAttribute>();
                if (info != null)
                {
                    Action<IContext, AmStack> eval;

                    var candidate = (Action<IContext, AmStack>)Delegate.CreateDelegate(typeof(Action<IContext, AmStack>), method, true);
                    if (info.InArity >= 0)
                    {
                        var arity = info.InArity;
                        var checkedEval = new Action<IContext, AmStack>((context, stack) =>
                        {
                            if (stack.Count < arity)
                            {
                                throw new StackException($"ill: stack underflow - expected at least {arity} arguments (got {stack.Count})");
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
                    goal.Define(Symbol.Get(info.Name), builtin, true, true);
                    foreach (var alias in info.Aliases)
                    {
                        goal.Define(Symbol.Get(alias), builtin);
                    }
                }
            }
        }
    }
}
