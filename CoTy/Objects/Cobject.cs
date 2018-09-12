using System.Collections;
using System.Collections.Generic;

namespace CoTy.Objects
{
    public abstract partial class Cobject : IEnumerable<Cobject>
    {
        public static readonly dynamic Eval = new Evaluator();

        public virtual void Close(Context context, Stack stack)
        {
            stack.Push(this);
        }

        public virtual void Apply(Context context, Stack stack)
        {
            Close(context, stack);
        }

        public abstract IEnumerator<Cobject> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Evaluator : Cobject
        {
            public override IEnumerator<Cobject> GetEnumerator()
            {
                yield break;
            }
        }
    }

    public abstract class Cobject<TClr, TCoty> : Cobject
        where TCoty : Cobject<TClr, TCoty>
    {
        protected Cobject(TClr value)
        {
            Value = value;
        }

        public TClr Value { get; }

        public override bool Equals(object obj)
        {
            return obj is TCoty other && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override IEnumerator<Cobject> GetEnumerator()
        {
            yield return this;
        }
    }
}
