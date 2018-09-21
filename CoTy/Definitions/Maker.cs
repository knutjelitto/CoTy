using System;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public class Maker : Cobject
    {
        private readonly IScope into;

        public Maker(IScope into)
        {
            this.into = into;
        }

        private void Enter(Symbol name, Action<IScope, IStack> action)
        {
            var builtin = Builtin.From(name, action);
            this.into.Define(name, builtin, true, true);
        }

        public void Define(string name, Func<object> operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                var result = operation();
                stack.Push(result);
            }
        }

        public void Define(string name, Func<object, object> operation)
        {
            Enter(name, Action);

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

        public void Define(string name, Func<object, object, object> operation)
        {
            Enter(name, Action);

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

        public void Define(string name, Func<IScope, IStack, object> operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                var result = operation(scope, stack);

                stack.Push(result);
            }
        }

        public void Define(string name, Func<IScope, IStack, object, object> operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                var result = operation(scope, stack, value);
                stack.Push(result);
            }
        }

        public void Define(string name, Func<IScope, IStack, object, object, object> operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                var result = operation(scope, stack, value1, value2);
                stack.Push(result);
            }
        }

        public void Define(string name, Func<IScope, IStack, object, object, object, object> operation)
        {
            Enter(name, Action);

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

        public void Define(string name, Action operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                operation();
            }
        }

        public void Define(string name, Action<object> operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                operation(value);
            }
        }

        public void Define(string name, Action<IScope, IStack> operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                operation(scope, stack);
            }
        }

        public void Define(string name, Action<IScope, IStack, object> operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(1);
                var value = stack.Pop();
                operation(scope, stack, value);
            }
        }

        public void Define(string name, Action<IScope, IStack, object, object> operation)
        {
            Enter(name, Action);

            void Action(IScope scope, IStack stack)
            {
                stack.Check(2);
                var value2 = stack.Pop();
                var value1 = stack.Pop();
                operation(scope, stack, value1, value2);
            }
        }

        public void Define(string name, Action<IScope, IStack, object, object, object> operation)
        {
            Enter(name, Action);

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
