using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CoTy.Ambiance;

namespace CoTy.Objects
{
    public abstract class CoTuple : IEnumerable<CoTuple>
    {
        public abstract void Apply(AmScope scope, AmStack stack);
        public abstract void Eval(AmScope scope, AmStack stack);

        public abstract IEnumerator<CoTuple> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class CoTuple<TClr> : CoTuple
    {
        protected CoTuple(TClr value)
        {
            Value = value;
        }

        public TClr Value { get; }

        public override void Apply(AmScope scope, AmStack stack)
        {
            stack.Push(this);
        }

        public override void Eval(AmScope scope, AmStack stack)
        {
        }

        public override IEnumerator<CoTuple> GetEnumerator()
        {
            yield return this;
        }
    }
}
