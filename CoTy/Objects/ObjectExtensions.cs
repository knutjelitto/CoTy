using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public static class ObjectExtensions
    {
        public static void Apply(this object This, IContext context, IStack stack)
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

        public static void Lambda(this object This, IContext context, IStack stack)
        {
            if (This is Cobject cvalue)
            {
                cvalue.Lambda(context, stack);
            }
            else
            {
                stack.Push(This);
            }
        }
    }
}
