using System;
using System.Collections.Generic;
using System.Text;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public interface IDefine
    {
        void Define(Symbol symbol, Cobject cobject);

        bool CanDefine(Symbol symbol);

        bool IsDefined(Symbol symbol);

    }
}
