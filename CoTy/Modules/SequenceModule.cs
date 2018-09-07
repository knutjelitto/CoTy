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

        [Builtin("upto")]
        private static void Upto(IContext context, AmStack stack)
        {
            IEnumerable<Cobject> Enumerate((dynamic from, dynamic upto) r)
            {
                while (r.from.LessOrEquals(r.upto) is Bool condition && condition)
                {
                    yield return r.from;

                    r.from = r.from.Succ();
                }
            }

            stack.Push(new Quotation(context, Enumerate(stack.Pop2()).ToList()));
        }

        [Builtin("count")]
        private static void Count(IContext context, AmStack stack)
        {
            stack.Push(Integer.From(stack.Pop().Count()));
        }

        [Builtin("reduce")]
        private static void Reduce(IContext context, AmStack stack)
        {
            var p = stack.Pop2();

            var first = true;
            foreach (var value in p.x)
            {
                stack.Push(value);
                if (!first)
                {
                    p.y.Execute(context, stack);
                }
                else
                {
                    first = false;
                }
            }
        }

        [Builtin("map")]
        private static void Map(IContext context, AmStack stack)
        {
            var p = stack.Pop2();

            Cobject Eval(Cobject value)
            {
                stack.Push(value);
                p.y.Execute(context, stack);
                return stack.Pop();
            }

            stack.Push(new Quotation(context, p.x.Select(Eval))); 
        }
    }
}
