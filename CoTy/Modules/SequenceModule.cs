using System.Collections.Generic;
using System.Linq;

using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class SequenceModule : Module
    {
        public SequenceModule(AmScope parent) : base(parent)
        {
        }

        [Builtin("flatten")]
        private static void Flatten(AmScope scope, AmStack stack)
        {
            IEnumerable<CoTuple> Enumerate(CoTuple tuple)
            {
                if (tuple is Quotation quotation)
                {
                    foreach (var inner in tuple)
                    {
                        foreach (var inner2 in Enumerate(inner))
                        {
                            yield return inner2;
                        }
                    }
                }
                else
                {
                    yield return tuple;
                }
            }

            stack.Push(new Quotation(Enumerate(stack.Pop())));
        }

        [Builtin("upto")]
        private static void Upto(AmScope scope, AmStack stack)
        {
            var upto = (dynamic)stack.Pop();
            var from = (dynamic)stack.Pop();

            IEnumerable<CoTuple> Enumerate()
            {
                while (from.LE(upto) is Bool condition && condition)
                {
                    yield return from;

                    from = from.Succ();
                }
            }

            stack.Push(new Quotation(Enumerate().ToList()));
        }
    }
}
