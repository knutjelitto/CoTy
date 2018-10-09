using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public sealed class SequenceLiteral : Cobject<List<Cobject>>, IEnumerable<Cobject>
    {
        private SequenceLiteral(List<Cobject> values) : base(values)
        {
        }

        public static SequenceLiteral From(IEnumerable<Cobject> values)
        {
            return new SequenceLiteral(values.ToList());
        }

        public static SequenceLiteral From(params Cobject[] values)
        {
            return From(values.AsEnumerable());
        }

        public static SequenceLiteral Quote(Cobject value)
        {
            return From(Enumerable.Repeat(value, 1));
        }

        public override void Eval(IScope scope, IStack stack)
        {
            stack.Push(Block.From(scope, Value));
        }

        public IEnumerator<Cobject> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public bool AllSymbols()
        {
            return Value.All(value => value is Symbol);
        }

        public bool IsEmpty()
        {
            return !Value.Any();
        }

        public override string ToString()
        {
            return $"({string.Join(" ", Value)})";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
