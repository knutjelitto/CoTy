using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class IntegerModule : Module
    {
        public IntegerModule(AmScope parent) : base(parent)
        {
            Define(CoSymbol.Get("+"), Add);
        }

        private void Add(AmScope scope, AmStack stack)
        {
            var i2 = (CoInteger)stack.Pop();
            var i1 = (CoInteger)stack.Pop();

            stack.Push(new CoInteger(i1.Value + i2.Value));
        }
    }
}
