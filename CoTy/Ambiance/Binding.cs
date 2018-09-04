using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class Binding
    {
        public Binding(Symbol symbol, CoTuple value)
        {
            Symbol = symbol;
            Value = value;
        }

        public Symbol Symbol { get; }
        public CoTuple Value { get; }
    }
}
