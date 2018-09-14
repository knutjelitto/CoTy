using System.Collections.Generic;

namespace CoTy.Objects
{
    public class Table : Cobject<Dictionary<Symbol, object>>
    {
        public Table()
            : base(new Dictionary<Symbol, object>())
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Table other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
