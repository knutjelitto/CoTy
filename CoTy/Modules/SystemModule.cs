using System;

using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class SystemModule : Module
    {
        public SystemModule(AmScope parent) : base(parent, "system")
        {
        }

        [Builtin("exit")]
        private static void Exit(IContext context, AmStack stack)
        {
            Environment.Exit(12);
        }
    }
}
