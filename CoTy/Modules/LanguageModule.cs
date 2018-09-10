using System;
using System.IO;

using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class LanguageModule : Module
    {
        public LanguageModule(AmScope parent) : base(parent, "language")
        {
        }

        [Builtin("apply")]
        private static void Apply(AmScope context, AmStack stack)
        {
            var value = stack.Pop();
            value.Apply(context, stack);
        }

        [Builtin("dequote")]
        private static void DeQuote(AmScope context, AmStack stack)
        {
            foreach (var value in stack.Pop())
            {
                stack.Push(value);
            }
        }

        [Builtin("if")]
        private static void If(AmScope context, AmStack stack)
        {
            var ifElse = stack.Pop();
            var ifTrue = stack.Pop();
            var condition = stack.Pop();

            condition.Apply(context, stack);
            var result = stack.Pop();

            if (result is Bool boolean && boolean.Value)
            {
                ifTrue.Apply(context, stack);
            }
            else
            {
                ifElse.Apply(context, stack);
            }
        }

        [Builtin("load-module", "lm", InArity = 1)]
        private static void LoadModule(AmScope context, AmStack stack)
        {
            Module.Load(context, stack, ".comy");
        }

        [Builtin("curry", IsOpaque = false)]  // a quot1 ⇒ quot2
        private static void Curry(AmScope context, AmStack stack)
        {
            var p = stack.Pop2();

            stack.Push(new Closure(context, p.x, p.y, Symbol.ApplySym));
        }
    }
}
