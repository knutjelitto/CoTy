using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public static class ObjectExtensions
    {
        public static void Apply(this object This, Context context, Stack stack)
        {
            if (This is Cobject cvalue)
            {
                cvalue.Apply(context, stack);
            }
            else
            {
                stack.Push(This);
            }
        }

        public static void Close(this object This, Context context, Stack stack)
        {
            if (This is Cobject cvalue)
            {
                cvalue.Close(context, stack);
            }
            else
            {
                stack.Push(This);
            }
        }
    }
}
