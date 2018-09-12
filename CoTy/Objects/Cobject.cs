using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using CoTy.Errors;

namespace CoTy.Objects
{
    public abstract partial class Cobject : DynamicObject, IEnumerable<Cobject>
    {
        public virtual void Close(Context context, Stack stack)
        {
            stack.Push(this);
        }

        public virtual void Apply(Context context, Stack stack)
        {
            Close(context, stack);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var member = GetType().GetMethod(binder.Name, args.Select(arg => arg.GetType()).ToArray());
            if (member != null)
            {
                result = member.Invoke(this, args);
                return true;
            }
            var parameters = string.Join(",", args.Select(arg => arg.GetType().Name));
            throw new BinderException($"`{binder.Name}´ not defined on `{GetType().Name}´ for `{parameters}´");
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
