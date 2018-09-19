using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace CoTy.Objects
{
    public abstract class Cobject : DynamicObject
    {
        public virtual void Lambda(IScope scope, IStack stack)
        {
            stack.Push(this);
        }

        public virtual void Apply(IScope scope, IStack stack)
        {
            Lambda(scope, stack);
        }

        protected static IEnumerable<T> Enumerate<T>(T value)
        {
            return value as IEnumerable<T> ?? Enumerable.Repeat(value, 1);
        }

#if false
        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new ForwardDynamicMetaObject(base.GetMetaObject(parameter));
        }

        private class ForwardDynamicMetaObject : DynamicMetaObject
        {
            private readonly DynamicMetaObject forward;

            public ForwardDynamicMetaObject(DynamicMetaObject forward) : base(forward.Expression, forward.Restrictions, forward.Value)
            {
                this.forward = forward;
            }

            public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
            {
                return this.forward.BindBinaryOperation(binder, arg);
            }

            public override DynamicMetaObject BindConvert(ConvertBinder binder)
            {
                return this.forward.BindConvert(binder);
            }

            public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
            {
                return this.forward.BindCreateInstance(binder, args);
            }

            public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
            {
                return this.forward.BindDeleteIndex(binder, indexes);
            }

            public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
            {
                return this.forward.BindDeleteMember(binder);
            }

            public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
            {
                return this.forward.BindGetIndex(binder, indexes);
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                return this.forward.BindGetMember(binder);
            }

            public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
            {
                return this.forward.BindInvoke(binder, args);
            }

            public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
            {
                return this.forward.BindInvokeMember(binder, args);
            }

            public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
            {
                return this.forward.BindSetIndex(binder, indexes, value);
            }

            public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
            {
                return this.forward.BindSetMember(binder, value);
            }

            public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
            {
                var dyn = this.forward.BindUnaryOperation(binder);

                return dyn;
            }

            public override bool Equals(object obj)
            {
                return this.forward.Equals(obj);
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return this.forward.GetDynamicMemberNames();
            }

            public override int GetHashCode()
            {
                return this.forward.GetHashCode();
            }

            public override string ToString()
            {
                return this.forward.ToString();
            }
        }
#endif
    }

    public abstract class Cobject<TClr> : Cobject
    {
        protected Cobject(TClr value)
        {
            Value = value;
        }

        protected TClr Value { get; }
    }
}
