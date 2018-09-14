using System;
using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects
{
    public partial class Cobject
    {
        protected static readonly Impl.CobjectImpl Impl = new Impl.CobjectImpl();
        protected static dynamic Dyn => Impl;

        public virtual void Close(Context context, Stack stack)
        {
            stack.Push(this);
        }

        public virtual void Apply(Context context, Stack stack)
        {
            Close(context, stack);
        }

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

    public abstract class Cobject<TClr> : Cobject
    {
        protected Cobject(TClr value)
        {
            Value = value;
        }

        public TClr Value { get; }
    }
}
