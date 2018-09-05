using System.Collections.Generic;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public partial class Quotation : Cobject<IEnumerable<Cobject>>
    {
        public Quotation(AmScope lexical, params Cobject[] objs)
            : this(lexical, (IEnumerable<Cobject>)objs)
        {
        }

        public Quotation(AmScope lexical, IEnumerable<Cobject> objs)
            : base(objs)
        {
            Lexical = lexical;
        }

        public override void Execute(AmScope scope, AmStack stack)
        {
            var inner = new AmScope(scope, "activation");

            foreach (var value in this)
            {
                var toEval = value;
                if (value is QuotationLiteral quotationLiteral)
                {
                    toEval = new Quotation(scope, quotationLiteral);
                }
                toEval.Eval(inner, stack);
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

        public AmScope Lexical { get; }
    }
}
