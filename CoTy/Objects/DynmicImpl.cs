// ReSharper disable UnusedMember.Global

using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using CoTy.Errors;

namespace CoTy.Objects
{
    public class DynmicImpl : IDynamicMetaObjectProvider
    {
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new Dynamic(parameter, this);
        }

        private class Dynamic : DynamicMetaObject
        {
            private static readonly Type voidType = typeof(void);

            public Dynamic(Expression expression, object value) : base(expression, BindingRestrictions.Empty, value)
            {
            }

            public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
            {
                var method = Value.GetType().GetMethod(binder.Name, args.Select(arg => arg.LimitType).ToArray());

                var defaulting = base.BindInvokeMember(binder, args);

                if (method == null)
                {
                    var error = Expression.Throw(
                        Expression.Constant(new DynaException(binder.Name, binder.ReturnType, args.Select(arg => arg.LimitType).ToArray())),
                        binder.ReturnType);

                    var notFound = new DynamicMetaObject(
                        error,
                        defaulting.Restrictions);

                    return notFound;
                }

                var self = Expression.Convert(Expression, LimitType);
                var parameters = args.Select(arg => Expression.Convert(arg.Expression, arg.LimitType));

                Expression call = Expression.Call(self, method, parameters);

                if (binder.ReturnType != voidType && method.ReturnType == voidType)
                {
                    call = Expression.Block(call, Expression.Default(binder.ReturnType));
                }
                else if (binder.ReturnType != method.ReturnType)
                {
                    call = Expression.Convert(call, binder.ReturnType);
                }

                var methodInfo = new DynamicMetaObject(
                    call,
                    defaulting.Restrictions);

                return methodInfo;
            }
        }

        Action<IScope, IStack> MakeBinaryOp(Func<dynamic, dynamic, object> binaryOp)
        {
            var scope = Expression.Parameter(typeof(IScope), "scope");
            var stack = Expression.Parameter(typeof(IStack), "stack");
            var value1 = Expression.Parameter(typeof(object), "value1");
            var value2 = Expression.Parameter(typeof(object), "value2");
            var result = Expression.Parameter(typeof(object), "result");
            var block = Expression.Block(
                new ParameterExpression[] { value1, value2, result },
                Expression.Call(Expression.Constant(this), "CheckStack", Type.EmptyTypes, stack, Expression.Constant(2)),
                Expression.Assign(value2, Expression.Call(stack, "Pop", Type.EmptyTypes)),
                Expression.Assign(value1, Expression.Call(stack, "Pop", Type.EmptyTypes)),
                Expression.Assign(result, Expression.Call(Expression.Constant(binaryOp.Target), binaryOp.Method, value1, value2)),
                Expression.Call(stack, "Push", Type.EmptyTypes, result));
            var lambda = Expression.Lambda<Action<IScope, IStack>>(block, scope, stack);
            var action = lambda.Compile();
            return action;
        }


        Action<IScope, IStack> MakeUnaryOp(Func<dynamic, object> unaryOp)
        {
            var scope = Expression.Parameter(typeof(IScope), "scope");
            var stack = Expression.Parameter(typeof(IStack), "stack");
            var value1 = Expression.Parameter(typeof(object), "value");
            var result = Expression.Parameter(typeof(object), "result");
            var block = Expression.Block(
                new ParameterExpression[] { value1, result },
                Expression.Assign(value1, Expression.Call(stack, "Pop", Type.EmptyTypes)),
                Expression.Assign(result, Expression.Call(Expression.Constant(unaryOp.Target), unaryOp.Method, value1)),
                Expression.Call(stack, "Push", Type.EmptyTypes, result));
            var lambda = Expression.Lambda<Action<IScope, IStack>>(block, scope, stack);
            var action = lambda.Compile();
            return action;
        }

    }
}
