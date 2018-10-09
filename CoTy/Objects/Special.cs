using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoTy.Objects
{
    public class Special : Cobject<Action<IScope, IStack>>
    {
        private Special(Action<IScope, IStack> eval)
            : base(eval)
        {
        }

        public override void Eval(IScope scope, IStack stack)
        {
            Value(scope, stack);
        }

        public static Special Define(Symbol symbol, Cobject value)
        {
            return new Special(
                (scope, stack) =>
                {
                    value.Eval(scope, stack);
                    value = stack.Pop();
                    scope.Define(symbol, value);
                });
        }

        public static Special MultiDefine(IEnumerable<Cobject> values)
        {
            var symbols = values.Cast<Symbol>().Reverse().ToList();

            return new Special(
                (scope, stack) =>
                {
                    stack.Check(symbols.Count);
                    foreach (var symbol in symbols)
                    {
                        var value = stack.Pop();
                        scope.Define(symbol, value);
                    }
                });
        }

        public static Special SingleDefine(Cobject symbol)
        {
            return MultiDefine(Enumerable.Repeat(symbol, 1));
        }

        public static Special MultiAssign(IEnumerable<Cobject> values)
        {
            var symbols = values.Cast<Symbol>().Reverse().ToList();

            return new Special(
                (scope, stack) =>
                {
                    stack.Check(symbols.Count);
                    foreach (var symbol in symbols)
                    {
                        var value = stack.Pop();
                        scope.Update(symbol, value);
                    }
                });
        }

        public static Special MultiAssign(object symbol)
        {
            return MultiAssign(Enumerable.Repeat(symbol, 1));
        }
    }
}
