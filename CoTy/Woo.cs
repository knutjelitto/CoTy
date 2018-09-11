using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using CoTy.Objects;

namespace CoTy
{
    public static class Woo
    {
        public static void Doo()
        {
        }

        public static void Doo2()
        {
            var binder = new Binder("doo", false, new CallInfo(3, "This", "context", "stack"));
            var type = typeof(Action<CallSite, Cobject, Context, Stack>);
            var arg0 = Expression.Parameter(typeof(Cobject), "this");
            var arg1 = Expression.Parameter(typeof(Context), "context");
            var arg2 = Expression.Parameter(typeof(Stack), "stack");
            var x = Expression.MakeDynamic(type, binder, arg0, arg1, arg2);

            var y = Expression.Lambda<Action<Cobject, Context, Stack>>(x, arg0, arg1, arg2);

            var z = y.Compile();

            var context = Context.Root("root");
            var stack = new Stack();

            z(Integer.One, context, stack);

        }

        private class Binder : InvokeMemberBinder
        {
            public Binder(string name, bool ignoreCase, CallInfo callInfo) : base(name, ignoreCase, callInfo)
            {
            }

            public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
            {
                throw new NotImplementedException();
            }

            public override DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
            {
                throw new NotImplementedException();
            }
        }
    }
}
