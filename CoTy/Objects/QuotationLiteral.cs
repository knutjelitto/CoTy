using System.Collections.Generic;
using CoTy.Ambiance;

namespace CoTy.Objects
{
    public partial class QuotationLiteral : Cobject<IEnumerable<Cobject>>
    {
        public QuotationLiteral(params Cobject[] objs)
            : this((IEnumerable<Cobject>)objs)
        {
        }

        public QuotationLiteral(IEnumerable<Cobject> objs)
            : base(objs)
        {
        }

        public override void Eval(IContext context, AmStack stack)
        {
            var quotation = new Quotation(context, Value);
            stack.Push(quotation);
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
