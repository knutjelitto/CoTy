using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public partial class Sequence : Cobject<IEnumerable<Cobject>, Sequence>
    {
        public Sequence(params Cobject[] objs)
            : this((IEnumerable<Cobject>)objs)
        {
        }

        public Sequence(IEnumerable<Cobject> objs)
            : base(objs)
        {
        }

        public override void Close(Context context, Stack stack)
        {
            stack.Push(new Closure(context, Value));
        }

        public bool TryGetQuotedSymbol(out Symbol symbol)
        {
            if (this.FirstOrDefault() is Symbol soleSymbol)
            {
                symbol = soleSymbol;
                return true;
            }

            symbol = null;
            return false;
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
