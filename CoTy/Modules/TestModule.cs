using System;
using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class TestModule : Module
    {
        public TestModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("assert")]
        private static void Assert(AmScope scope, AmStack stack)
        {
            var expectedQuot = stack.Pop();
            expectedQuot.Eval(scope, stack);
            var expected = (dynamic) stack.Pop();

            var actualQuot = stack.Pop();
            actualQuot.Eval(scope, stack);
            var actual = (dynamic)stack.Pop();

            var equals = (CoBoolean) actual.EQ(expected);

            if (!equals.Value)
            {
                Console.WriteLine($"expected");
                Console.WriteLine($"{expected}");
                Console.WriteLine($"but got");
                Console.WriteLine($"{actual} <= {actualQuot}");
            }
        }
    }
}
