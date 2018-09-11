using System.Collections.Generic;
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

        [Builtin("up", InArity = 1)]
        private static void Up(Context context, Stack stack)
        {
            var from = stack.Popd();
            
            stack.Push(from.Up(from));
        }

        [Builtin("upto", InArity = 2)]
        private static void Upto(Context context, Stack stack)
        {
            var to = stack.Pop();
            var from = stack.Popd();

            stack.Push(from.Upto(from, to));
        }

        [Builtin("take", InArity = 2)]
        private static void Take(Context context, Stack stack)
        {
            var count = stack.Pop();
            var sequence = stack.Popd();

            stack.Push(sequence.Take(sequence, count));
        }

        [Builtin("skip", InArity = 2)]
        private static void Skip(Context context, Stack stack)
        {
            var count = stack.Pop();
            var sequence = stack.Popd();

            stack.Push(sequence.Skip(sequence, count));
        }

        [Builtin("count")]
        private static void Count(Context context, Stack stack)
        {
            var sequence = stack.Popd();

            stack.Push(sequence.Count(sequence));
        }

        [Builtin("repeat", InArity = 2)]
        private static void Repeat(Context context, Stack stack)
        {
            var count = stack.Pop();
            var value = stack.Popd();

            stack.Push(value.Repeat(value, count));
        }

        [Builtin("reduce")]
        private static void Reduce(Context context, Stack stack)
        {
            var p = stack.Pop2();

            var first = true;
            foreach (var value in p.x)
            {
                stack.Push(value);
                if (!first)
                {
                    p.y.Apply(context, stack);
                }
                else
                {
                    first = false;
                }
            }
        }

        [Builtin("map")]
        private static void Map(Context context, Stack stack)
        {
            var p = stack.Pop2();

            Cobject Eval(Cobject value)
            {
                stack.Push(value);
                p.y.Apply(context, stack);
                return stack.Pop();
            }

            stack.Push(new Sequence(p.x.Select(Eval)));
        }

        [Builtin("each")]
        private static void Each(Context context, Stack stack)
        {
            var p = stack.Pop2();

            foreach (var value in p.x)
            {
                stack.Push(value);
                p.y.Apply(context, stack);
            }
        }
    }
}
