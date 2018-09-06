using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
namespace CoTy.Modules
{
    public class BindingModule : Module
    {
        private static Symbol GetSymbol(Cobject toDefine)
        {
            Symbol symbol;

            if (toDefine is Chars str)
            {
                symbol = Symbol.Get(str.Value);
            }
            else if (!(toDefine is Quotation quotation) || !quotation.TryGetQuotedSymbol(out symbol))
            {
                throw new BinderException($"ill: expected string or quoted symbol do define a binding, found: `{toDefine}´");
            }

            return symbol;
        }

        [Builtin("def")]
        private static void Define(IContext context, AmStack stack)
        {
            var symbol = GetSymbol(stack.Pop());
            var value = stack.Pop();


            context.Define(symbol, value);
        }

        [Builtin("undef")]
        private static void Undefine(IContext context, AmStack stack)
        {
            var symbol = GetSymbol(stack.Pop());

            context.Undefine(symbol);
        }

        [Builtin("set")]
        private static void Set(IContext context, AmStack stack)
        {
            var symbol = GetSymbol(stack.Pop());
            var value = stack.Pop();


            context.Update(symbol, value);
        }

        [Builtin("def?")]
        private static void DefinedPred(IContext context, AmStack stack)
        {
            var symbol = GetSymbol(stack.Pop());

            stack.Push(Bool.From(context.IsDefined(symbol)));
        }
    }
}
