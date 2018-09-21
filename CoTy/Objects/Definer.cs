using System.Collections.Generic;
using System.Linq;
using CoTy.Support;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Definer : SymbolsActioner
    {
        private Definer(IEnumerable<Symbol> objs)
            : base(objs, Symbol.BindTo)
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

        protected override void ActionOnValue(IScope scope, Symbol symbol, object value)
        {
            scope.Define(symbol, value);
        }
    }
}
