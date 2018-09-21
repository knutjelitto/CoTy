using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Sources;
using CoTy.Support;

// ReSharper disable RedundantAssignment
namespace CoTy.Objects
{
    public class Assigner : SymbolsActioner
    {
        private Assigner(IEnumerable<Symbol> objs)
            : base(objs, Symbol.Assign)
        {
        }

        public static Assigner From(IEnumerable<Symbol> values)
        {
            return new Assigner(values);
        }

        public static Assigner From(params Symbol[] objs)
        {
            return From(objs.AsEnumerable());
        }

        protected override void ActionOnValue(IScope scope, Symbol symbol, object value)
        {
            scope.Update(symbol, value);
        }
    }
}
