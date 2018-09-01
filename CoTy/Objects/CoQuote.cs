using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public class CoQuote : CoMutator, IEnumerable<CoObject>
    {
        private List<CoObject> Value { get;  }

        public CoQuote(params CoObject[] objs)
            : this((IEnumerable<CoObject>)objs)
        {
        }

        public CoQuote(IEnumerable<CoObject> objs)
        {
            Value = objs.ToList();
        }

        public IEnumerator<CoObject> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return "(" + string.Join(" ", Value) + ")";
        }

        public override void Apply(AmScope scope, AmStack stack)
        {
            var newScope = new AmScope(scope);
            foreach (var @object in this)
            {
                @object.Apply(newScope, stack);
            }
        }
    }
}
