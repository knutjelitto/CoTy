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

        private static void Outcome(dynamic expected, Context context, Stack stack)
        {
            var actualQuot = stack.Pop();
            actualQuot.Apply(context, stack);
            var actual = (dynamic)stack.Pop();

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
            expectedQuot.Apply(context, stack);
            var expected = (dynamic) stack.Pop();

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
            var p = stack.Pop2();
            Console.WriteLine($"{p.x}");
            foreach (var quotation in p.y)
            {
                quotation.Close(context, stack);
            }
        }
    }
}
