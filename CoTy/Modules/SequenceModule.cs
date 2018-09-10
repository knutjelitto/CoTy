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
        public SequenceModule(AmScope parent) : base(parent, "sequence")
        {
        }

        [Builtin("upup")]
        private static void UpUp(AmScope context, AmStack stack)
        {
            IEnumerable<Cobject> Loop(dynamic from)
            {
                while (true)
                {
                    yield return from;

                    from = from.Succ();
                }
            }

            stack.Push(new Sequence(Loop(stack.Popd())));
        }

        [Builtin("upto")]
        private static void Upto(AmScope context, AmStack stack)
        {
            IEnumerable<Cobject> Enumerate((dynamic from, dynamic upto) r)
            {
                while (r.from.LessOrEquals(r.upto) is Bool condition && condition)
                {
                    yield return r.from;

                    r.from = r.from.Succ();
                }
            }

            stack.Push(new Closure(context, Enumerate(stack.Pop2()).ToList()));
        }

        [Builtin("count")]
        private static void Count(AmScope context, AmStack stack)
        {
            stack.Push(Integer.From(stack.Pop().Count()));
        }

        [Builtin("reduce")]
        private static void Reduce(AmScope context, AmStack stack)
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
        private static void Map(AmScope context, AmStack stack)
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
        private static void Each(AmScope context, AmStack stack)
        {
            var p = stack.Pop2();

            foreach (var value in p.x)
            {
                stack.Push(value);
                p.y.Apply(context, stack);
            }
        }

        [Builtin("times")]
        private static void Times(AmScope context, AmStack stack)
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
