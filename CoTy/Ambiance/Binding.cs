using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class Binding
    {
        public Binding(AmScope scope, Symbol symbol, Cobject value, bool isSealed, bool isOpaque)
        {
            Scope = scope;
            Symbol = symbol;
            Value = value;
            IsSealed = isSealed;
            IsOpaque = isOpaque;
        }

        public AmScope Scope { get; }
        public Symbol Symbol { get; }
        public Cobject Value { get; set; }
        public bool IsSealed { get; }
        public bool IsOpaque { get; }
    }
}
