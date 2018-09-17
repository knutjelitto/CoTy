using System;
using System.Reflection;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class Module : Cobject
    {
        protected Module(string name)
        {
            Name = name;
        }

        public string Name { get; }

        protected static bool TryGetSymbol(object candidate, out Symbol symbol)
        {
            if (candidate is Characters str)
            {
                symbol = Symbol.Get(str.Value);
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

        public virtual Context Reflect(Context into)
        {
            Reflect(GetType(), into);
            return into;
        }

        private static void Reflect(Type moduleType, Context into)
        {
            foreach (var method in moduleType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                var info = method.GetCustomAttribute<BuiltinAttribute>();
                if (info != null)
                {
                    Action<Context, Stack> eval;

                    var candidate = (Action<Context, Stack>)Delegate.CreateDelegate(typeof(Action<Context, Stack>), method, true);
                    if (info.InArity >= 0)
                    {
                        var arity = info.InArity;
                        var checkedEval = new Action<Context, Stack>((context, stack) =>
                        {
                            if (stack.Value.Count < arity)
                            {
                                throw new StackException(arity, stack.Value.Count);
                            }
                            candidate(context, stack);
                        });
                        eval = checkedEval;
                    }
                    else
                    {
                        eval = candidate;
                    }
                    var builtin = new Builtin(eval);
                    into.Define(Symbol.Get(info.Name), builtin, true, info.IsOpaque);
                    foreach (var alias in info.Aliases)
                    {
                        into.Define(Symbol.Get(alias), builtin);
                    }
                }
            }
        }

        protected class BuiltinAttribute : Attribute
        {
            public BuiltinAttribute(string name, params string[] aliases)
            {
                Name = name;
                Aliases = aliases;
            }

            public string Name { get; }
            public string[] Aliases { get; }
            public int InArity { get; set; } = -1;
            public int OutArity { get; set; } = -1;
            public bool IsOpaque { get; set; } = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------- */

        private void CheckStack(Stack stack, int atLeast)
        {
            if (stack.Value.Count < atLeast)
            {
                throw new StackException(atLeast, stack.Value.Count);
            }
        }

        private Action<Context, Stack> MakeConstant(object constantValue)
        {
            Action<Context, Stack> action = (context, stack) => stack.Push(constantValue);
            return action;
        }

        private Action<Context, Stack> MakeUnaryOp(Func<object, object> binaryOp)
        {
            Action<Context, Stack> action =
                (context, stack) =>
                {
                    CheckStack(stack, 1);
                    var value = stack.Pop();
                    var result = binaryOp(value);
                    stack.Push(result);
                };
            return action;
        }

        private Action<Context, Stack> MakeBinaryOp(Func<object, object, object> binaryOp)
        {
            Action<Context, Stack> action =
                (context, stack) =>
                {
                    CheckStack(stack, 2);
                    var value2 = stack.Pop();
                    var value1 = stack.Pop();
                    var result = binaryOp(value1, value2);
                    stack.Push(result);
                };
            return action;
        }

        private Action<Context, Stack> MakeAction(Action<Context, Stack> operation)
        {
            Action<Context, Stack> action =
                (context, stack) =>
                {
                    operation(context, stack);
                };
            return action;
        }

        private Action<Context, Stack> MakeAction(Action<Context, Stack, object> operation)
        {
            Action<Context, Stack> action =
                (context, stack) =>
                {
                    CheckStack(stack, 1);
                    var value = stack.Pop();
                    operation(context, stack, value);
                };
            return action;
        }

        private Action<Context, Stack> MakeAction(Action<Context, Stack, object, object> operation)
        {
            Action<Context, Stack> action =
                (context, stack) =>
                {
                    CheckStack(stack, 2);
                    var value2 = stack.Pop();
                    var value1 = stack.Pop();
                    operation(context, stack, value1, value2);
                };
            return action;
        }

        private Action<Context, Stack> MakeAction(Action<Context, Stack, object, object, object> operation)
        {
            Action<Context, Stack> action =
                (context, stack) =>
                {
                    CheckStack(stack, 2);
                    var value3 = stack.Pop();
                    var value2 = stack.Pop();
                    var value1 = stack.Pop();
                    operation(context, stack, value1, value2, value3);
                };
            return action;
        }

        private Action<Context, Stack> MakeAction(object argument)
        {
            if (argument is Delegate operation)
            {
                var argcount = operation.Method.GetParameters().Length;
                bool voidable = operation.Method.ReturnType == typeof(void);

                Action<Context, Stack> action =
                    (context, stack) =>
                    {
                        CheckStack(stack, argcount);
                        var args = new object[argcount];
                        for (var i = argcount - 1; i >= 0; --i)
                        {
                            args[i] = stack.Pop();
                        }
                        var result = operation.DynamicInvoke(args);
                        stack.Push(result);
                    };
                return action;
            }
            else
            {
                return MakeConstant(argument);
            }
        }

        protected void Enter(Context into, string name, Action<Context, Stack> action)
        {
            var builtin = new Builtin(action);
            into.Define(Symbol.Get(name), builtin, true, true);
        }

        protected void Define(Context into, string name, Func<object, object, object> binOp)
        {
            Enter(into, name, MakeBinaryOp(binOp));
        }

        protected void Define(Context into, string name, Func<object, object> operation)
        {
            Enter(into, name, MakeUnaryOp(operation));
        }

        protected void Define(Context into, string name, object constantValue)
        {
            Enter(into, name, MakeConstant(constantValue));
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
