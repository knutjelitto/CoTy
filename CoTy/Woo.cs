using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using CoTy.Objects;

namespace CoTy
{
    public class Woo : IDynamicMetaObjectProvider
    {
        public static void Doo()
        {
            ExpressionType x;

            BigInteger y;
        }

        public static void Doo2()
        {
            var woo = new Woo();
            var chars1 = Characters.From("a");
            var chars2 = Characters.From("b");
            var int1 = Integer.From(11);
            var int2 = Integer.From(12);

            woo.Call(woo, chars1, chars2);
            woo.Call(woo, chars1, chars2);
            //((dynamic)woo).Add(chars1, chars2);
            //((dynamic)woo).Add(int1, int2);
            //((dynamic)woo).Add((Cobject)chars1, chars2);
            //((dynamic)woo).Add(chars1, chars2);

            Environment.Exit(12);
        }

        private void Call(Woo woo, Characters chars1, Characters chars2)
        {
            ((dynamic)woo).Add(chars1, chars2);
        }

        public void Add(Characters value2, Characters value)
        {
        }

        public void Add(Integer value1, Integer value2)
        {
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MyDynamicMetaObject(parameter, this);
        }

        private class MyDynamicMetaObject : DynamicMetaObject
        {
            private static Type voidType = typeof(void);

            public MyDynamicMetaObject(Expression expression, object value) : base(expression, BindingRestrictions.Empty, value)
            {
            }

            public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
            {
                var method = Value.GetType().GetMethod(binder.Name, args.Select(arg => arg.RuntimeType).ToArray());

                var defaulting = base.BindInvokeMember(binder, args);

                if (method == null)
                {
                    return defaulting;
                }

                var self = Expression.Convert(Expression, LimitType);
                // ReSharper disable once AssignNullToNotNullAttribute
                var parameters = args.Select(arg => Expression.Convert(arg.Expression, arg.RuntimeType));

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
    }
}
