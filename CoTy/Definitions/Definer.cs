using System;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public abstract class Definer : Cobject
    {
        protected Definer(string name)
        {
            Name = name;
        }

        public string Name { get; }

        protected static bool TryGetSymbol(object candidate, out Symbol symbol)
        {
            if (!(candidate is Sequence sequence) || !sequence.TryGetQuotedSymbol(out symbol))
            {
                symbol = null;
                return false;
            }

            return true;
        }

        protected static Symbol GetSymbol(object candidate)
        {
            if (!TryGetSymbol(candidate, out var symbol))
            {
                throw new BinderException($"`{string.Join(" ", Enumerate(candidate))}´ can't be a symbol");
            }

            return symbol;
        }

        private static void Enter(IContext into, string name, Action<IContext, IStack> action)
        {
            var builtin = Builtin.From(action);
            into.Define(name, builtin, true, true);
        }

        public abstract void Define(IContext into);

        /* ---------------------------------------------------------------------------------------------------------------------- */

        protected void Define(IContext into, string name, Func<object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                var result = operation();
                stack.Push(result);
            }
        }

        protected void Define(IContext into, string name, Func<object, object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                value.Apply(context, stack);
                value = stack.Pop();
                var result = operation(value);
                stack.Push(result);
            }
        }

        protected void Define(IContext into, string name, Func<object, object, object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                stack.Check(2);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                value1.Apply(context, stack);
                value1 = stack.Pop();
                value2.Apply(context, stack);
                value2 = stack.Pop();
                var result = operation(value1, value2);
                stack.Push(result);
            }
        }

        protected void Define(IContext into, string name, Func<IContext, IStack, object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                var result = operation(context, stack);

                stack.Push(result);
            }
        }

        protected void Define(IContext into, string name, Func<IContext, IStack, object, object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                var result = operation(context, stack, value);
                stack.Push(result);
            }
        }


        protected void Define(IContext into, string name, Action operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                operation();
            }
        }

        protected void Define(IContext into, string name, Action<object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                operation(value);
            }
        }

        protected void Define(IContext into, string name, Action<IContext, IStack> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                operation(context, stack);
            }
        }

        protected void Define(IContext into, string name, Action<IContext, IStack, object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                operation(context, stack, value);
            }
        }

        protected void Define(IContext into, string name, Action<IContext, IStack, object, object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                stack.Check(2);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                operation(context, stack, value1, value2);
            }
        }

        protected void Define(IContext into, string name, Action<IContext, IStack, object, object, object> operation)
        {
            Enter(into, name, Action);

            void Action(IContext context, IStack stack)
            {
                stack.Check(2);
                var value3 = stack.Pop();
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                operation(context, stack, value1, value2, value3);
            }

        }
    }
}
