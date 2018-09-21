using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public class Define : Cobject
    {
        private Define(Symbol symbol, object value)
        {
            Symbol = symbol;
            Value = value;
        }

        public Symbol Symbol { get; }
        public object Value { get; }

        public static Define From(Symbol symbol, object value)
        {
            return new Define(symbol, value);
        }

        public override void Eval(IScope scope, IStack stack)
        {
            scope.Define(Symbol, Value);
        }
    }
}
