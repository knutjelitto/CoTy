using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

using CoTy.Ambiance;
using CoTy.Errors;

namespace CoTy.Objects
{
    public abstract partial class Cobject : DynamicObject, IEnumerable<Cobject>
    {
        public virtual void Close(AmScope context, AmStack stack)
        {
            stack.Push(this);
        }

        public virtual void Apply(AmScope context, AmStack stack)
        {
            Close(context, stack);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            throw new BinderException($"`{binder.Name}´ not defined for `{GetType().Name}´ and `{args[0].GetType().Name}´");
        }

        public abstract IEnumerator<Cobject> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
