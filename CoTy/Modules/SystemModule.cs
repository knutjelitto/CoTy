using System;

using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class SystemModule : Module
    {
        [Builtin("exit")]
        private static void Exit(IContext context, AmStack stack)
        {
            Environment.Exit(12);
        }
    }
}
