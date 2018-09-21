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
            Eval(stack);
        }

        public IBinder Eval(IStack stack)
        {
            var binder = Binder.From("local");
            var localScope = Scope.Chain(binder);
            foreach (var value in Value)
            {
                value.Eval(localScope, stack);
            }
            return binder;
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

        public override string ToString()
        {
            return "{" + string.Join(" ", Value) + "}";
        }
    }
}
