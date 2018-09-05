using System.Collections.Generic;
using System.Linq;
using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class SequenceModule : Module
    {
        [Builtin("upto")]
        private static void Upto(AmScope scope, AmStack stack)
        {
            IEnumerable<Cobject> Enumerate((dynamic from, dynamic upto) r)
            {
                while (r.from.LessOrEqual(r.upto) is Bool condition && condition)
                {
                    yield return r.from;

                    r.from = r.from.Succ();
                }
            }

            stack.Push(new Quotation(scope, Enumerate(stack.Pop2()).ToList()));
        }

        [Builtin("count")]
        private static void Count(AmScope scope, AmStack stack)
        {
            stack.Push(Integer.From(stack.Pop().Count()));
        }
    }
}
