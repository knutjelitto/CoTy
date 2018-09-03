using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class LanguageModule : Module
    {
        public LanguageModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("define")]
        private static void Define(AmScope scope, AmStack stack)
        {
            var toDefine = stack.Pop();
            var definition = stack.Pop();

            CoSymbol symbol;

            if (toDefine is CoString str)
            {
                symbol = CoSymbol.Get(str.Value);
            }
            else
            {
                symbol = (CoSymbol)toDefine;
            }

            scope.Define(symbol, definition);
        }

        [Builtin("quote")]
        private static void Quote(AmScope scope, AmStack stack)
        {
            var @object = stack.Pop();
            var quotation = new CoQuotation(@object);
            stack.Push(quotation);
        }

        [Builtin("if")]
        private static void If(AmScope scope, AmStack stack)
        {
            var ifElse = stack.Pop();
            var ifTrue = stack.Pop();
            var condition = stack.Pop();

            condition.Eval(scope, stack);
            var result = stack.Pop();

            if (result is CoBoolean boolean && boolean.Value)
            {
                ifTrue.Eval(scope, stack);
            }
            else
            {
                ifElse.Eval(scope, stack);
            }
        }

        [Builtin("reduce")]
        private static void Reduce(AmScope scope, AmStack stack)
        {
            var operation = stack.Pop();
            var values = stack.Pop();

            var first = true;
            foreach (var value in values)
            {
                stack.Push(value);
                if (!first)
                {
                    operation.Eval(scope, stack);
                }
                else
                {
                    first = false;
                }
            }
        }
    }
}
