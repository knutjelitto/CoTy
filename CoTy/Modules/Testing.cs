using System;
using CoTy.Ambiance;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class Testing : Module
    {
        public Testing(AmScope parent) : base(parent, "test")
        {
        }

        private static void Outcome(dynamic expected, AmScope context, AmStack stack)
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
        private static void Assert(AmScope context, AmStack stack)
        {
            var expectedQuot = stack.Pop();
            expectedQuot.Apply(context, stack);
            var expected = (dynamic) stack.Pop();

            Outcome(expected, context, stack);
        }

        [Builtin("assert-true")]
        private static void IsTrue(AmScope context, AmStack stack)
        {
            var expected = Bool.True;
            Outcome(expected, context, stack);
        }

        [Builtin("assert-false")]
        private static void IsFalse(AmScope context, AmStack stack)
        {
            var expected = Bool.False;
            Outcome(expected, context, stack);
        }

        [Builtin("test")]
        private static void Test(AmScope context, AmStack stack)
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
