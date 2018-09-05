using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class Binding
    {
        public Binding(Symbol symbol, Cobject value, AmScope lexical)
        {
            Symbol = symbol;
            Value = value;
            Lexical = lexical;
        }

        public Symbol Symbol { get; }
        public Cobject Value { get; }
        public AmScope Lexical { get; }
    }
}
