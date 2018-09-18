using System;

using CoTy.Objects;
using CoTy.Support;

namespace CoTy.Definitions
{
    public class TestsDefiner : Definer
    {
        public TestsDefiner() : base("testing") { }

        public override void Define(IContext into)
        {
            Define(into,
                   "assert",
                   (context, stack, actual, expected) =>
                   {
                       expected.Apply(context, stack);
                       var expectedValue = stack.Pop();

                       Outcome(expectedValue, actual, context, stack);
                   });
            Define(into,
                   "assert-true",
                   (context, stack, actual) => { Outcome(true, actual, context, stack); });
            Define(into,
                   "assert-false",
                   (context, stack, actual) => { Outcome(false, actual, context, stack); });
        }

        private static void Outcome(object expectedValue, object actual, IContext context, IStack stack)
        {
            actual.Apply(context, stack);
            var actualValue = stack.Pop();

            var equals = actualValue.Equals(expectedValue);

            if (!equals)
            {
                G.C.WriteLine($"{expectedValue} != {actualValue} ;;{actual}");
            }
        }
    }
}
