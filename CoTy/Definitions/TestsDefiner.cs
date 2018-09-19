using System;

using CoTy.Objects;
using CoTy.Support;

namespace CoTy.Definitions
{
    public class TestsDefiner : Definer
    {
        public TestsDefiner() : base("testing") { }

        public override void Define(IScope into)
        {
            Define(into,
                   "assert",
                   (scope, stack, actual, expected) =>
                   {
                       expected.Apply(scope, stack);
                       var expectedValue = stack.Pop();

                       Outcome(expectedValue, actual, scope, stack);
                   });
            Define(into,
                   "assert-true",
                   (scope, stack, actual) => { Outcome(true, actual, scope, stack); });
            Define(into,
                   "assert-false",
                   (scope, stack, actual) => { Outcome(false, actual, scope, stack); });
        }

        private static void Outcome(object expectedValue, object actual, IScope scope, IStack stack)
        {
            actual.Apply(scope, stack);
            var actualValue = stack.Pop();

            var equals = actualValue.Equals(expectedValue);

            if (!equals)
            {
                G.C.WriteLine($"{expectedValue} != {actualValue} ;;{actual}");
            }
        }
    }
}
