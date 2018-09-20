using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public sealed class Pusher : Cobject
    {
        private Pusher()
        {
        }

        public override void Eval(IScope scope, IStack stack)
        {
            base.Eval(scope, stack);
        }

        public static Pusher From()
        {
            return new Pusher();
        }
    }
}
