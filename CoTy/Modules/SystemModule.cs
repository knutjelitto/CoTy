using System;

using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class SystemModule : Module
    {
        public SystemModule(Context parent) : base(parent, "system")
        {
        }

        [Builtin("exit", InArity = 1)]
        private static void Exit(Context context, Stack stack)
        {
            Environment.Exit(!(stack.Pop() is Integer exit) ? 99 : (int)exit.Value);
        }
    }
}
