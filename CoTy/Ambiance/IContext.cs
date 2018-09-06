using System;
using System.Collections.Generic;
using System.Text;

using CoTy.Objects;

namespace CoTy.Ambiance
{
    public interface IContext
    {
        AmScope Local { get; }
        AmScope Lexical { get; }

        IContext WithLocal(AmScope localScope);
        IContext WithLexical(AmScope lexicalContext);

        IContext WithLocal();
        IContext WithLexical();

        bool IsDefined(Symbol symbol);
        bool CanDefine(Symbol symbol);
        bool CanUpdate(Symbol symbol);

        void Define(Symbol symbol, Cobject value, bool isSealed = false, bool isOpaque = false);
        void Update(Symbol symbol, Cobject value);
        void Undefine(Symbol symbol);

        void Get(Symbol symbol, out Cobject value);
    }
}
