using System;
using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class TestModule : Module
    {
        public TestModule(AmScope parent) : base(parent, "test")
        {
        }

        private static void Outcome(dynamic expected, IContext context, AmStack stack)
        {
            var actualQuot = stack.Pop();
            actualQuot.Execute(context, stack);
            var actual = (dynamic)stack.Pop();

            var equals = (Bool)actual.Equals(expected);

            if (!equals.Value)
            {
                Console.WriteLine($"{expected} != {actual} ;;{actualQuot}");
            }
        }

        [Builtin("assert")]
        private static void Assert(IContext context, AmStack stack)
        {
            var expectedQuot = stack.Pop();
            expectedQuot.Execute(context, stack);
            var expected = (dynamic) stack.Pop();

            Outcome(expected, context, stack);
        }

        [Builtin("assert-true")]
        private static void IsTrue(IContext context, AmStack stack)
        {
            var expected = Bool.True;
            Outcome(expected, context, stack);
        }

        [Builtin("assert-false")]
        private static void IsFalse(IContext context, AmStack stack)
        {
            var expected = Bool.False;
            Outcome(expected, context, stack);
        }
    }
}
