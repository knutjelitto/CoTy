using System.Collections.Generic;
using System.Linq;
using CoTy.Ambiance;
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

        [Builtin("up")]
        private static void Up(Context context, AmStack stack)
        {
            stack.Push(Sequence.Up(stack.Popd()));
        }

        [Builtin("upto")]
        private static void Upto(Context context, AmStack stack)
        {
            var p = stack.Pop2d();

            stack.Push(Sequence.Upto(p.x, p.y));
        }

        [Builtin("take")]
        private static void Take(Context context, AmStack stack)
        {
            var p = stack.Pop2();

            IEnumerable<Cobject> Loop()
            {
                var seq = p.x;
                var cnt = (dynamic)p.y;

                foreach (var value in seq)
                {
                    if (cnt.Greater(Integer.Zero))
                    {
                        yield return value;

                        cnt = cnt.Pred();
                    }
                    else
                    {
                        break;
                    }
                }

            }

            stack.Push(new Sequence(Loop()));
        }

        [Builtin("count")]
        private static void Count(Context context, AmStack stack)
        {
            stack.Push(Integer.From(stack.Pop().Count()));
        }

        [Builtin("reduce")]
        private static void Reduce(Context context, AmStack stack)
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
        private static void Map(Context context, AmStack stack)
        {
            var p = stack.Pop2();

            Cobject Eval(Cobject value)
            {
                stack.Push(value);
                p.y.Apply(context, stack);
                return stack.Pop();
            }

            stack.Push(new Closure(context, p.x.Select(Eval)));
        }

        [Builtin("each")]
        private static void Each(Context context, AmStack stack)
        {
            var p = stack.Pop2();

            foreach (var value in p.x)
            {
                stack.Push(value);
                p.y.Apply(context, stack);
            }
        }

        [Builtin("times")]
        private static void Times(Context context, AmStack stack)
        {
            IEnumerable<Cobject> Loop(Cobject value, dynamic count)
            {
                while (count.Greater(Integer.Zero))
                {
                    yield return value;

                    count = count.Pred();
                }
            }

            var p = stack.Pop2();

            stack.Push(new Closure(context, Loop(p.x, p.y)));
        }
    }
}
