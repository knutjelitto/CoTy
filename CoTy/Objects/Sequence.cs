using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public class Sequence : Cobject<IEnumerable<Cobject>>, IEnumerable<Cobject>
    {
        protected Sequence(IEnumerable<Cobject> objs)
            : base(objs)
        {
        }

        protected override void Close(Context context, Stack stack)
        {
            stack.Push(Closure.From(context, Value));
        }

        public static Sequence From(Cobject obj)
        {
            return From(Enumerable.Repeat(obj, 1));
        }

        public static Sequence From(params Cobject[] objs)
        {
            return From((IEnumerable<Cobject>)objs);
        }

        public static Sequence From(IEnumerable<Cobject> objs)
        {
            return new Sequence(objs);
        }

        public bool TryGetQuotedSymbol(out Symbol symbol)
        {
            if (Value.FirstOrDefault() is Symbol soleSymbol && !Value.Skip(1).Any())
            {
                symbol = soleSymbol;
                return true;
            }

            symbol = null;
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is Sequence other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public IEnumerator<Cobject> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public override string ToString()
        {
            return "(" + string.Join(" ", Value) + ")";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
