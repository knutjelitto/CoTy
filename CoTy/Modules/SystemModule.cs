using System;

using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class SystemModule : Module
    {
        public SystemModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("exit")]
        private static void Exit(AmScope scope, AmStack stack)
        {
            Environment.Exit(12);
        }
    }
}
