using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public abstract class CoObject : IEnumerable<CoObject>
    {
        public abstract void Apply(AmScope scope, AmStack stack);
        public abstract void Eval(AmScope scope, AmStack stack);

        public abstract IEnumerator<CoObject> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class CoObject<TClr> : CoObject
    {
        protected CoObject(TClr value)
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
            stack.Push(this);
        }

        public override IEnumerator<CoObject> GetEnumerator()
        {
            yield return this;
        }
    }
}
