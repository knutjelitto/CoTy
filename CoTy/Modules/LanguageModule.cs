﻿using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
namespace CoTy.Modules
{
    public class LanguageModule : Module
    {

        [Builtin("exec")]
        private static void Execute(AmScope scope, AmStack stack)
        {
            var value = stack.Pop();
            value.Execute(scope, stack);
        }

        [Builtin("quote")]
        private static void Quote(AmScope scope, AmStack stack)
        {
            var value = stack.Pop();
            var quotation = new Quotation(scope, value);
            stack.Push(quotation);
        }

        [Builtin("dequote")]
        private static void DeQuote(AmScope scope, AmStack stack)
        {
            foreach (var value in stack.Pop())
            {
                stack.Push(value);
            }
        }

        [Builtin("if")]
        private static void If(AmScope scope, AmStack stack)
        {
            var ifElse = stack.Pop();
            var ifTrue = stack.Pop();
            var condition = stack.Pop();

            condition.Execute(scope, stack);
            var result = stack.Pop();

            if (result is Bool boolean && boolean.Value)
            {
                ifTrue.Execute(scope, stack);
            }
            else
            {
                ifElse.Execute(scope, stack);
            }
        }

        [Builtin("reduce")]
        private static void Reduce(AmScope scope, AmStack stack)
        {
            var operation = stack.Pop();
            var values = stack.Pop();

            var first = true;
            foreach (var value in values)
            {
                stack.Push(value);
                if (!first)
                {
                    operation.Execute(scope, stack);
                }
                else
                {
                    first = false;
                }
            }
        }
    }
}
