using System;
using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects.Implementations
{
    public partial class Implementation
    {
        public dynamic Dyn => this;

        protected static IEnumerable<T> Enumerate<T>(T value)
        {
            return value as IEnumerable<T> ?? Enumerable.Repeat(value, 1);
        }

        public static void Apply(Context context, Stack stack, object value)
        {
            if (value is Cobject cvalue)
            {
                cvalue.Apply(context, stack);
            }
            else
            {
                throw new NotImplementedException();
                //stack.Push(value);
            }
        }

        public static void Close(Context context, Stack stack, object value)
        {
            if (value is Cobject cvalue)
            {
                cvalue.Close(context, stack);
            }
            else
            {
                throw new NotImplementedException();
                //stack.Push(value);
            }
        }

    }
}
