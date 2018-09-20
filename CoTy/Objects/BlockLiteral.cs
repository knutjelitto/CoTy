using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public sealed class BlockLiteral : Cobject<List<object>>
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

        public override string ToString()
        {
            return "{" + string.Join(" ", Value) + "}";
        }
    }
}
