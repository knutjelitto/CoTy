using System;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public class Definer : Cobject
    {
        protected Definer(string name)
        {
            Name = name;
        }

        public string Name { get; }

        protected static bool TryGetSymbol(object candidate, out Symbol symbol)
        {
            if (candidate is Characters str)
            {
                symbol = Symbol.Get(str);
            }
            else if (!(candidate is Sequence sequence) || !sequence.TryGetQuotedSymbol(out symbol))
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

        private static void Enter(Context into, string name, Action<Context, Stack> action)
        {
            var builtin = Builtin.From(action);
            into.Define(Symbol.Get(name), builtin, true, true);
        }

        public virtual Context Define(Context into)
        {
            return into;
        }

        /* ---------------------------------------------------------------------------------------------------------------------- */

        private static void CheckStack(Stack stack, int atLeast)
        {
            if (stack.Count < atLeast)
            {
                throw new StackException(atLeast, stack.Count);
            }
        }

        private Action<Context, Stack> MakeConstant(object constantValue)
        {
            void Action(Context context, Stack stack) => stack.Push(constantValue);

            return Action;
        }

        private Action<Context, Stack> MakeUnaryOp(Func<object, object> binaryOp)
        {
            void Action(Context context, Stack stack)
            {
                CheckStack(stack, 1);
                var value = stack.Pop();
                var result = binaryOp(value);
                stack.Push(result);
            }

            return Action;
        }

        private Action<Context, Stack> MakeBinaryOp(Func<object, object, object> binaryOp)
        {
            void Action(Context context, Stack stack)
            {
                CheckStack(stack, 2);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                var result = binaryOp(value1, value2);
                stack.Push(result);
            }

            return Action;
        }

        private Action<Context, Stack> MakeAction(Action operation)
        {
            void Action(Context context, Stack stack)
            {
                operation();
            }

            return Action;
        }

        private Action<Context, Stack> MakeAction(Func<Context, Stack, object> operation)
        {
            void Action(Context context, Stack stack)
            {
                var result = operation(context, stack);

                stack.Push(result);
            }

            return Action;
        }

        private Action<Context, Stack> MakeAction(Func<Context, Stack, object, object> operation)
        {
            void Action(Context context, Stack stack)
            {
                CheckStack(stack, 1);
                var value = stack.Pop();
                var result = operation(context, stack, value);
                stack.Push(result);
            }

            return Action;
        }

        private Action<Context, Stack> MakeAction(Action<object> operation)
        {
            void Action(Context context, Stack stack)
            {
                CheckStack(stack, 1);
                var value = stack.Pop();
                operation(value);
            }

            return Action;
        }

        private Action<Context, Stack> MakeAction(Action<Context, Stack> operation)
        {
            void Action(Context context, Stack stack)
            {
                operation(context, stack);
            }

            return Action;
        }

        private Action<Context, Stack> MakeAction(Action<Context, Stack, object> operation)
        {
            void Action(Context context, Stack stack)
            {
                CheckStack(stack, 1);
                var value = stack.Pop();
                operation(context, stack, value);
            }

            return Action;
        }

        private Action<Context, Stack> MakeAction(Action<Context, Stack, object, object> operation)
        {
            void Action(Context context, Stack stack)
            {
                CheckStack(stack, 2);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                operation(context, stack, value1, value2);
            }

            return Action;
        }

        private Action<Context, Stack> MakeAction(Action<Context, Stack, object, object, object> operation)
        {
            void Action(Context context, Stack stack)
            {
                CheckStack(stack, 2);
                var value3 = stack.Pop();
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                operation(context, stack, value1, value2, value3);
            }

            return Action;
        }

        protected void Define(Context into, string name, Func<Context, Stack, object> operation)
        {
            Enter(into, name, MakeAction(operation));
        }

        protected void Define(Context into, string name, Func<object, object, object> binOp)
        {
            Enter(into, name, MakeBinaryOp(binOp));
        }

        protected void Define(Context into, string name, Func<Context, Stack, object, object> operation)
        {
            Enter(into, name, MakeAction(operation));
        }


        protected void Define(Context into, string name, Func<object, object> operation)
        {
            Enter(into, name, MakeUnaryOp(operation));
        }

        protected void Define(Context into, string name, object constantValue)
        {
            Enter(into, name, MakeConstant(constantValue));
        }

        protected void Define(Context into, string name, Action operation)
        {
            Enter(into, name, MakeAction(operation));
        }

        protected void Define(Context into, string name, Action<object> operation)
        {
            Enter(into, name, MakeAction(operation));
        }

        protected void Define(Context into, string name, Action<Context, Stack> operation)
        {
            Enter(into, name, MakeAction(operation));
        }

        protected void Define(Context into, string name, Action<Context, Stack, object> operation)
        {
            Enter(into, name, MakeAction(operation));
        }

        protected void Define(Context into, string name, Action<Context, Stack, object, object> operation)
        {
            Enter(into, name, MakeAction(operation));
        }

        protected void Define(Context into, string name, Action<Context, Stack, object, object, object> operation)
        {
            Enter(into, name, MakeAction(operation));
        }
    }
}
