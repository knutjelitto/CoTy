using System.Collections.Generic;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public partial class Quotation : Cobject<IEnumerable<Cobject>>
    {
        public Quotation(params Cobject[] objs)
            : this((IEnumerable<Cobject>)objs)
        {
        }

        public Quotation(IEnumerable<Cobject> objs)
            : base(objs)
        {
        }
        public override void Execute(AmScope scope, AmStack stack)
        {
            var inner = new AmScope(new AmFrame(scope.Activation, "activation"), scope.Lexical);

            foreach (var value in this)
            {
                value.Eval(inner, stack);
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
