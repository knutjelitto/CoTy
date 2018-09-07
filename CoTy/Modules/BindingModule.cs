using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class BindingModule : Module
    {
        public BindingModule(AmScope parent) : base(parent, "binding")
        {
        }

        private static bool TryGetSymbol(Cobject toDefine, out Symbol symbol)
        {
            if (toDefine is Chars str)
            {
                symbol = Symbol.Get(str.Value);
            }
            else if (!(toDefine is Quotation quotation) || !quotation.TryGetQuotedSymbol(out symbol))
            {
                symbol = null;
                return false;
            }

            return true;
        }

        private static Symbol GetSymbol(Cobject toDefine)
        {
            if (!TryGetSymbol(toDefine, out var symbol))
            {
                throw new BinderException($"`{toDefine}´ can't be a symbol");
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
            if (TryGetSymbol(stack.Pop(), out var symbol))
            {
                stack.Push((Bool) context.IsDefined(symbol));
            }
            stack.Push(Bool.False);
        }
    }
}
