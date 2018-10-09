using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public class Sequence : Cobject<IEnumerable<Cobject>>, IEnumerable<Cobject>
    {
        public static readonly Sequence Empty = From();

        private Sequence(IEnumerable<Cobject> objs)
            : base(objs)
        {
        }

        public static Sequence From(IEnumerable<Cobject> values)
        {
            return new Sequence(values);
        }

        public static Sequence From(params Cobject[] values)
        {
            return From(values.AsEnumerable());
        }

        public override bool Equals(object obj)
        {
            return obj is Sequence other && Value.SequenceEqual(other.Value);
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

        private static IEnumerable<object> Loop(IEnumerator<Cobject> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
