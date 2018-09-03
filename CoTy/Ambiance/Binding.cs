using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class Binding
    {
        public Binding(CoSymbol symbol, CoTuple value)
        {
            Symbol = symbol;
            Value = value;
        }

        public CoSymbol Symbol { get; }
        public CoTuple Value { get; }
    }
}
