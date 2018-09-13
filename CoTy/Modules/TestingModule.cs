using System;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class TestingModule : Module
    {
        public TestingModule(Context parent) : base(parent, "test")
        {
        }

        private static void Outcome(object expected, Context context, Stack stack)
        {
            var actualQuot = stack.Pop();
            Apply(context, stack, actualQuot);
            var actual = stack.Pop();

            var equals = (Bool)actual.Equals(expected);

            if (!equals.Value)
            {
                Console.WriteLine($"{expected} != {actual} ;;{actualQuot}");
            }
        }

        [Builtin("assert")]
        private static void Assert(Context context, Stack stack)
        {
            var expectedQuot = stack.Pop();
            Apply(context, stack, expectedQuot);
            var expected = stack.Pop();

            Outcome(expected, context, stack);
        }

        [Builtin("assert-true")]
        private static void IsTrue(Context context, Stack stack)
        {
            var expected = Bool.True;
            Outcome(expected, context, stack);
        }

        [Builtin("assert-false")]
        private static void IsFalse(Context context, Stack stack)
        {
            var expected = Bool.False;
            Outcome(expected, context, stack);
        }

        [Builtin("test")]
        private static void Test(Context context, Stack stack)
        {
            var description = stack.Pop();
            var sequence = Enumerate(stack.Pop());

            Console.WriteLine($"{description}");
            foreach (var value in sequence)
            {
                Close(context, stack, value);
            }
        }
    }
}
