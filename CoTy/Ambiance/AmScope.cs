using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public struct AmScope
    {
        public readonly AmFrame Activation;
        public readonly AmFrame Lexical;

        public AmScope(AmFrame activation, AmFrame lexical)
        {
            this.Activation = activation;
            this.Lexical = lexical;
        }

        public void Define(Symbol symbol, Cobject value)
        {
            this.Activation.Define(symbol, value);
        }

        public void Find(Symbol symbol, out Binding binding)
        {
            this.Lexical.Find(symbol, out binding);
        }
    }
}
