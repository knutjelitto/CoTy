using System;
using System.Collections.Generic;
using System.Text;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public struct AmContext
    {
        public AmContext(AmScope lexical, AmScope local)
        {
            Lexical = lexical;
            Local = local;
        }

        public AmScope Lexical { get; }
        public AmScope Local { get; }

        public bool CanDefine(Symbol symbol)
        {
            return Lexical.CanDefine(symbol) && Local.CanDefine(symbol);
        }

        public bool CanUpdate(Symbol symbol)
        {
            if (Local.TryFind(symbol, out var binding) || Lexical.TryFind(symbol, out binding))
            {
                return !binding.IsSealed;
            }
            return false;
        }

        public void Define(Symbol symbol, Cobject value, bool isSealed = false, bool isOpaque = false)
        {
            if (CanDefine(symbol))
            {
                Local.Define(symbol, value, isSealed, isOpaque);
            }
            else
            {
                throw new BinderException($"can't define `{symbol}´");
            }
        }

        public void Get(Symbol symbol, out Cobject value)
        {
            if (Local.IsDefined(symbol))
            {
                Local.Get(symbol, out value);
            }
            else
            {
                Lexical.Get(symbol, out value);
            }
        }

        public bool IsDefined(Symbol symbol)
        {
            return Local.IsDefined(symbol) || Lexical.IsDefined(symbol);
        }

        public void Undefine(Symbol symbol)
        {
            throw new NotImplementedException();
        }

        public void Update(Symbol symbol, Cobject value)
        {
            if (Local.CanUpdate(symbol))
            {
                Local.Update(symbol, value);
            }
            else
            {
                Lexical.Update(symbol, value);
            }
        }
    }
}
