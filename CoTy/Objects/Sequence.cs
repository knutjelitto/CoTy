using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public class Sequence : Cobject<IEnumerable<object>>, IEnumerable<object>
    {
        protected Sequence(IEnumerable<object> objs)
            : base(objs)
        {
        }

        public override void Close(Context context, Stack stack)
        {
            stack.Push(Closure.From(context, Value));
        }

        public static Sequence From(object obj)
        {
            return From(Enumerable.Repeat(obj, 1));
        }

        public static Sequence From(params object[] objs)
        {
            return From((IEnumerable<object>)objs);
        }

        public static Sequence From(IEnumerable<object> objs)
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

        public bool AllSymbols()
        {
            return Value.All(value => value is Symbol);
        }

        public bool IsEmpty()
        {
            return !Value.Any();
        }

        public override bool Equals(object obj)
        {
            return obj is Sequence other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public IEnumerator<object> GetEnumerator()
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
