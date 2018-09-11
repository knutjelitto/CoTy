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
            var exit = stack.Pop<Integer>();

            Environment.Exit((int)exit.Value);
        }
    }
}
