using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using CoTy.Ambiance;
using CoTy.Errors;

namespace CoTy.Objects
{
    public abstract partial class Cobject : DynamicObject, IEnumerable<Cobject>
    {
        public virtual void Eval(IContext context, AmStack stack)
        {
            stack.Push(this);
        }

        public virtual void Execute(IContext context, AmStack stack)
        {
            Eval(context, stack);
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
