using System.Collections.Generic;
using System.Linq;
using CoTy.Support;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Definer : MultiSymbol
    {
        private Definer(IEnumerable<Symbol> objs)
            : base(objs, Symbol.Bind, (scope, symbol, value) => scope.Define(symbol, value))
        {
        }

        public static Definer From(IEnumerable<Symbol> values)
        {
            return new Definer(values);
        }

        public static Definer From(params Symbol[] objs)
        {
            return From(objs.AsEnumerable());
        }
    }
}
