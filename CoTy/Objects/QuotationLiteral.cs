using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public class QuotationLiteral : Cobject<IEnumerable<Cobject>>
    {
        public QuotationLiteral(params Cobject[] objs)
            : this((IEnumerable<Cobject>)objs)
        {
        }

        public QuotationLiteral(IEnumerable<Cobject> objs)
            : base(objs)
        {
        }

        public override IEnumerator<Cobject> GetEnumerator()
        {
            return Value.GetEnumerator();
        }
    }
}
