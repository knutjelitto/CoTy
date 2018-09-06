using System.Collections;
using System.Collections.Generic;

using CoTy.Ambiance;

namespace CoTy.Objects
{
    public abstract partial class Cobject : IEnumerable<Cobject>
    {
        public virtual void Eval(IContext context, AmStack stack)
        {
            stack.Push(this);
        }

        public virtual void Execute(IContext context, AmStack stack)
        {
            Eval(context, stack);
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

        public override IEnumerator<Cobject> GetEnumerator()
        {
            yield return this;
        }
    }
}
