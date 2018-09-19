using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public static class ObjectExtensions
    {
        public static void Apply(this object This, IScope scope, IStack stack)
        {
            if (This is Cobject cvalue)
            {
                cvalue.Apply(scope, stack);
            }
            else
            {
                stack.Push(This);
            }
        }

        public static void Lambda(this object This, IScope scope, IStack stack)
        {
            if (This is Cobject cvalue)
            {
                cvalue.Lambda(scope, stack);
            }
            else
            {
                stack.Push(This);
            }
        }

        public static IEnumerable<object> Enumerate(this object This)
        {
            if (This is Cobject cobject)
            {
                return cobject;
            }

            // ReSharper disable once UsePatternMatching
            var enumerable = This as IEnumerable<object>;

            // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
            if (enumerable == null)
            {
                enumerable = Enumerable.Repeat(This, 1);
            }

            return enumerable;
        }

    }
}
