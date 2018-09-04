using System.Collections.Generic;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public partial class Quotation : CoTuple<IEnumerable<CoTuple>>
    {
        public Quotation(params CoTuple[] objs)
            : this((IEnumerable<CoTuple>)objs)
        {
        }

        public Quotation(IEnumerable<CoTuple> objs)
            : base(objs)
        {
        }

        public override void Eval(AmScope scope, AmStack stack)
        {
            var inner = new AmScope(scope);

            foreach (var value in this)
            {
                value.Apply(inner, stack);
            }
        }

        public override IEnumerator<CoTuple> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public override string ToString()
        {
            return "(" + string.Join(" ", Value) + ")";
        }
    }
}
