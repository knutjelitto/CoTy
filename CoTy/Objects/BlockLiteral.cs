using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public sealed class BlockLiteral : Cobject<List<object>>, IEnumerable<object>
    {
        private BlockLiteral(List<object> values) : base(values)
        {
        }

        public static BlockLiteral From(IEnumerable<object> values)
        {
            return new BlockLiteral(values.ToList());
        }

        public override void Eval(IScope scope, IStack stack)
        {
            stack.Push(Block.From(scope, Value));
        }

        public IEnumerator<object> GetEnumerator()
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
