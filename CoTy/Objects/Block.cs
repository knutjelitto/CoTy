using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public sealed class Block : Cobject<List<object>>
    {
        private Block(IScope scope, List<object> values) : base(values)
        {
            Scope = scope;
        }

        public IScope Scope { get; }

        public static Block From(IScope scope, IEnumerable<object> values)
        {
            return new Block(scope, values.ToList());
        }

        public override void Eval(IScope scope, IStack stack)
        {
            var localScope = Scope.Chain(Binder.From("local"));
            foreach (var value in Value)
            {
                value.Eval(localScope, stack);
            }
        }

        public override string ToString()
        {
            return "{" + string.Join(" ", Value) + "}";
        }
    }
}
