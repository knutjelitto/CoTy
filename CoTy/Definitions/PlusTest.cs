using System;

using CoTy.Objects;
using CoTy.Support;

namespace CoTy.Definitions
{
    public class PlusTest : Core
    {
        public PlusTest() : base("testing") { }

        public override void Define(Maker into)
        {
            into.Define(
                "assert",
                (scope, stack, actual, expected) =>
                {
                    expected.Eval(scope, stack);
                    var expectedValue = stack.Pop();

                    Outcome(expectedValue, actual, scope, stack);
                });
            into.Define(
                "assert-true",
                (scope, stack, actual) => { Outcome(true, actual, scope, stack); });
            into.Define(
                "assert-false",
                (scope, stack, actual) => { Outcome(false, actual, scope, stack); });
        }

        private static void Outcome(object expectedValue, object actual, IScope scope, IStack stack)
        {
            actual.Eval(scope, stack);
            var actualValue = stack.Pop();

            var equals = actualValue.Equals(expectedValue);

            if (!equals)
            {
                G.C.WriteLine($"{expectedValue} != {actualValue} ;;{actual}");
            }
        }
    }
}
