using System;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public abstract class Core : Cobject
    {
        protected Core(string name)
        {
            Name = name;
        }

        public string Name { get; }

        private static void Enter(IScope into, Symbol name, Action<IScope, IStack> action)
        {
            var builtin = Builtin.From(name, action);
            into.Define(name, builtin, true, true);
        }

        public abstract void Define(Maker into);

        /* ---------------------------------------------------------------------------------------------------------------------- */

        protected void Define(IScope into, string name, Func<Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                var result = operation();
                stack.Push(result);
            }
        }

        protected void Define(IScope into, string name, Func<Cobject, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                value.Eval(scope, stack);
                value = stack.Pop();
                var result = operation(value);
                stack.Push(result);
            }
        }

        protected void Define(IScope into, string name, Func<Cobject, Cobject, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(2);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                value1.Eval(scope, stack);
                value1 = stack.Pop();
                value2.Eval(scope, stack);
                value2 = stack.Pop();
                var result = operation(value1, value2);
                stack.Push(result);
            }
        }

        protected void Define(IScope into, string name, Func<IScope, IStack, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                var result = operation(scope, stack);

                stack.Push(result);
            }
        }

        protected void Define(IScope into, string name, Func<IScope, IStack, Cobject, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                var result = operation(scope, stack, value);
                stack.Push(result);
            }
        }

        protected void Define(IScope into, string name, Func<IScope, IStack, Cobject, Cobject, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                var result = operation(scope, stack, value1, value2);
                stack.Push(result);
            }
        }

        protected void Define(IScope into, string name, Func<IScope, IStack, Cobject, Cobject, Cobject, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value3 = stack.Pop();
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                var result = operation(scope, stack, value1, value2, value3);
                stack.Push(result);
            }
        }

        protected void Define(IScope into, string name, Action operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                operation();
            }
        }

        protected void Define(IScope into, string name, Action<Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                operation(value);
            }
        }

        protected void Define(IScope into, string name, Action<IScope, IStack> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                operation(scope, stack);
            }
        }

        protected void Define(IScope into, string name, Action<IScope, IStack, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                operation(scope, stack, value);
            }
        }

        protected void Define(IScope into, string name, Action<IScope, IStack, Cobject, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(2);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                operation(scope, stack, value1, value2);
            }
        }

        protected void Define(IScope into, string name, Action<IScope, IStack, Cobject, Cobject, Cobject> operation)
        {
            Enter(into, name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(2);
                var value3 = stack.Pop();
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                operation(scope, stack, value1, value2, value3);
            }

        }
    }
}
