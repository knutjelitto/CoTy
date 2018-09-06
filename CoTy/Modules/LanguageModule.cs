using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
namespace CoTy.Modules
{
    public class LanguageModule : Module
    {

        [Builtin("exec")]
        private static void Execute(IContext context, AmStack stack)
        {
            var value = stack.Pop();
            value.Execute(context, stack);
        }

        [Builtin("quote")]
        private static void Quote(IContext context, AmStack stack)
        {
            var value = stack.Pop();
            var quotation = new Quotation(context.Lexical, value);
            stack.Push(quotation);
        }

        [Builtin("dequote")]
        private static void DeQuote(IContext context, AmStack stack)
        {
            foreach (var value in stack.Pop())
            {
                stack.Push(value);
            }
        }

        [Builtin("if")]
        private static void If(IContext context, AmStack stack)
        {
            var ifElse = stack.Pop();
            var ifTrue = stack.Pop();
            var condition = stack.Pop();

            condition.Execute(context, stack);
            var result = stack.Pop();

            if (result is Bool boolean && boolean.Value)
            {
                ifTrue.Execute(context, stack);
            }
            else
            {
                ifElse.Execute(context, stack);
            }
        }

        [Builtin("reduce")]
        private static void Reduce(IContext context, AmStack stack)
        {
            var operation = stack.Pop();
            var values = stack.Pop();

            var first = true;
            foreach (var value in values)
            {
                stack.Push(value);
                if (!first)
                {
                    operation.Execute(context, stack);
                }
                else
                {
                    first = false;
                }
            }
        }
    }
}
