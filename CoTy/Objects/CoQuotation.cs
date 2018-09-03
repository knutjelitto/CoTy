using System.Collections.Generic;
using System.Linq;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public class CoQuotation : CoObject<IEnumerable<CoObject>>
    {
        public CoQuotation(params CoObject[] objs)
            : this((IEnumerable<CoObject>)objs)
        {
        }

        public CoQuotation(IEnumerable<CoObject> objs)
            : base(objs)
        {
        }
        public override void Eval(AmScope scope, AmStack stack)
        {
            var inner = new AmScope(scope);

            foreach (var @object in this)
            {
                @object.Apply(inner, stack);
            }
        }

        public CoQuotation Add(CoObject other)
        {
            return new CoQuotation(this.Concat(other));
        }

        public CoQuotation CoAdd(CoObject other)
        {
            return new CoQuotation(other.Concat(this));
        }

        public override IEnumerator<CoObject> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public override string ToString()
        {
            return "(" + string.Join(" ", Value) + ")";
        }
    }
}
