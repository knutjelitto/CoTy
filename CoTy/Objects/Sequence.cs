using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public class Sequence : Cobject<IEnumerable<object>>, IEnumerable<object>
    {
        public static readonly Sequence Empty = From();

        protected Sequence(IEnumerable<object> objs)
            : base(objs)
        {
        }

        public static Sequence From(params object[] values)
        {
            return From(values.AsEnumerable());
        }

        public static Sequence From(IEnumerable<object> values)
        {
            return new Sequence(values);
        }

        public static Sequence From(IEnumerator<object> rest)
        {
            return new Sequence(Loop(rest));
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
            return obj is Sequence other && Value.SequenceEqual(other.Value);
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

        private static IEnumerable<object> Loop(IEnumerator<object> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
