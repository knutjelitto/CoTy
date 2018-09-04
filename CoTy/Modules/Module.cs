using System;
using System.Reflection;

using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Module : AmScope
    {
        public Module(AmScope parent) : base(parent)
        {
            Reflect();
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
            public int Arity { get; set; } = -1;
        }

        private void Reflect()
        {
            foreach (var method in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                var info = method.GetCustomAttribute<BuiltinAttribute>();
                if (info != null)
                {
                    Action<AmScope, AmStack> apply;

                    var candidate = (Action<AmScope, AmStack>)Delegate.CreateDelegate(typeof(Action<AmScope, AmStack>), method);
                    if (info.Arity >= 0)
                    {
                        var checkedApply = new Action<AmScope, AmStack>((scope, stack) =>
                        {
                            if (stack.Count < info.Arity)
                            {
                                throw new StackException($"ill: stack underflow - expected at least {info.Arity} arguments (got {stack.Count})");
                            }
                            candidate(scope, stack);
                        });
                        apply = checkedApply;
                    }
                    else
                    {
                        apply = candidate;
                    }
                    var builtin = new Builtin(apply);
                    Define(Symbol.Get(info.Name), builtin);
                    foreach (var alias in info.Aliases)
                    {
                        Define(Symbol.Get(alias), builtin);
                    }
                }
            }
        }
    }
}
