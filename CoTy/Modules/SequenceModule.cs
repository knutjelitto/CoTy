using System.Linq;

using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class SequenceModule : Module
    {
        public SequenceModule(Context parent) : base(parent, "sequence")
        {
        }

        [Builtin("up", InArity = 1, OutArity = 1)]
        private static void Up(Context context, Stack stack)
        {
            var from = stack.Pop();
            
            stack.Push(Dyn.Up(from));
        }

        [Builtin("upto", InArity = 2)]
        private static void Upto(Context context, Stack stack)
        {
            var to = stack.Pop();
            var from = stack.Pop();

            stack.Push(Dyn.Upto(from, to));
        }

        [Builtin("range", InArity = 2)]
        private static void Range(Context context, Stack stack)
        {
            var count = stack.Pop();
            var from = stack.Pop();

            stack.Push(Dyn.Range(from, count));
        }

        [Builtin("take", InArity = 2)]
        private static void Take(Context context, Stack stack)
        {
            var count = stack.Pop();
            var sequence = stack.Pop();

            stack.Push(Dyn.Take(sequence, count));
        }

        [Builtin("skip", InArity = 2)]
        private static void Skip(Context context, Stack stack)
        {
            var count = stack.Pop();
            var sequence = stack.Pop();

            stack.Push(Dyn.Skip(sequence, count));
        }

        [Builtin("count")]
        private static void Count(Context context, Stack stack)
        {
            var sequence = stack.Pop();

            stack.Push(Dyn.Count(sequence));
        }

        [Builtin("repeat", InArity = 2)]
        private static void Repeat(Context context, Stack stack)
        {
            var count = stack.Pop();
            var value = stack.Pop();

            stack.Push(Dyn.Repeat(value, count));
        }

        [Builtin("reduce")]
        private static void Reduce(Context context, Stack stack)
        {
            var action = stack.Pop();
            var sequence = stack.Pop();

            Dyn.Reduce(context, stack, sequence, action);
        }

        [Builtin("map", InArity = 2, OutArity = 1)]
        private static void Map(Context context, Stack stack)
        {
            var action = stack.Pop();
            var sequence = stack.Pop();

            Dyn.Map(context, stack, sequence, action);
        }

        [Builtin("each")]
        private static void Each(Context context, Stack stack)
        {
            var action = stack.Pop();
            var sequence = stack.Pop();

            Dyn.Each(context, stack, sequence, action);
        }
    }
}
