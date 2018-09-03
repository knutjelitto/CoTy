using System;
using System.Reflection;

using CoTy.Ambiance;
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
        }

        private void Reflect()
        {
            Console.WriteLine($"reflecting {this.GetType().Name}");
            foreach (var method in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                var info = method.GetCustomAttribute<BuiltinAttribute>();
                if (info != null)
                {
                    Console.WriteLine($"{method.Name} -> {info.Name}");

                    var apply = (Action<AmScope, AmStack>)Delegate.CreateDelegate(typeof(Action<AmScope, AmStack>), method);
                    var builtin = new Builtin(apply);
                    Define(CoSymbol.Get(info.Name), builtin);
                    foreach (var alias in info.Aliases)
                    {
                        Define(CoSymbol.Get(alias), builtin);
                    }
                }
            }
        }
    }
}
