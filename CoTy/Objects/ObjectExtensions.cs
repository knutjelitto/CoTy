using System.Collections.Generic;
using System.Linq;
using CoTy.Errors;

namespace CoTy.Objects
{
    public static class ObjectExtensions
    {
        public static void Eval(this object This, IScope scope, IStack stack)
        {
            if (This is Cobject cvalue)
            {
                cvalue.Eval(scope, stack);
            }
            else
            {
                stack.Push(This);
            }
        }

        public static IEnumerable<object> Enumerate(this object This)
        {
            // ReSharper disable once UsePatternMatching
            var enumerable = This as IEnumerable<object>;

            // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
            if (enumerable == null)
            {
                enumerable = Enumerable.Repeat(This, 1);
            }

            return enumerable;
        }

        public static bool TryGetSymbol(this object This, out Symbol symbol)
        {
            if (!(This is Block block) || !block.TryGetQuotedSymbol(out symbol))
            {
                symbol = null;
                return false;
            }

            return true;
        }

        public static Symbol GetSymbol(this object This)
        {
            if (!TryGetSymbol(This, out var symbol))
            {
                throw new BinderException($"`{This}´ can't be a symbol");
            }

            return symbol;
        }
    }
}
