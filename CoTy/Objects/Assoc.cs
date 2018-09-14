using System.Collections.Generic;

namespace CoTy.Objects
{
    public class Assoc : Cobject<Dictionary<Symbol, object>>
    {
        public Assoc()
            : base(new Dictionary<Symbol, object>())
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Assoc other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
