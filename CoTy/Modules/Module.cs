using System;
using System.Collections.Generic;
using System.Text;

using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Module : AmScope
    {
        public Module(AmScope parent) : base(parent)
        {
        }

        protected void Define(CoSymbol symbol, Action<AmScope, AmStack> apply)
        {
            Define(symbol, new Builtin(apply));
        }
    }
}
