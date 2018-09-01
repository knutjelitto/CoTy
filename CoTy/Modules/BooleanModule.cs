using CoTy.Ambiance;
using CoTy.Objects;

namespace CoTy.Modules
{
    public class BooleanModule : Module
    {
        public BooleanModule(AmScope parent) : base(parent)
        {
            Define(CoSymbol.True, (sc, st) => st.Push(CoBoolean.True));
            Define(CoSymbol.False, (sc, st) => st.Push(CoBoolean.False));
        }
    }
}
