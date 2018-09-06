using System;
using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class TestModule : Module
    {
        [Builtin("assert")]
        private static void Assert(IContext context, AmStack stack)
        {
            var expectedQuot = stack.Pop();
            expectedQuot.Execute(context, stack);
            var expected = (dynamic) stack.Pop();

            var actualQuot = stack.Pop();
            actualQuot.Execute(context, stack);
            var actual = (dynamic)stack.Pop();

            var equals = (Bool) actual.Equal(expected);

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
