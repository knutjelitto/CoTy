using System.Collections;
using System.Collections.Generic;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public abstract partial class Cobject : IEnumerable<Cobject>
    {
        public abstract void Eval(AmScope scope, AmStack stack);

        public virtual void Execute(AmScope scope, AmStack stack)
        {
            Eval(scope, stack);
        }

        public abstract IEnumerator<Cobject> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class Cobject<TClr> : Cobject
    {
        protected Cobject(TClr value)
        {
            Value = value;
        }

        public TClr Value { get; }

        public override void Eval(AmScope scope, AmStack stack)
        {
            stack.Push(this);
        }

        public override IEnumerator<Cobject> GetEnumerator()
        {
            yield return this;
        }
    }
}
