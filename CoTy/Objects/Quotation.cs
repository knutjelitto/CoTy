﻿using System.Collections.Generic;
using System.Linq;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public partial class Quotation : Cobject<IEnumerable<Cobject>>
    {
        public Quotation(IContext lexical, params Cobject[] objs)
            : this(lexical, (IEnumerable<Cobject>)objs)
        {
        }

        public Quotation(IContext lexical, IEnumerable<Cobject> objs)
            : base(objs)
        {
            Lexical = lexical;
        }

        private IContext Lexical { get; }

        public bool TryGetQuotedSymbol(out Symbol symbol)
        {
            if (this.SingleOrDefault() is Symbol soleSymbol)
            {
                symbol = soleSymbol;
                return true;
            }

            symbol = null;
            return false;
        }

        public override void Execute(IContext context, AmStack stack)
        {
            context = Lexical.WithLocal();
            foreach (var value in this)
            {
                value.Eval(context, stack);
            }
        }

        public override IEnumerator<Cobject> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public override string ToString()
        {
            return "(" + string.Join(" ", Value) + ")";
        }
    }
}