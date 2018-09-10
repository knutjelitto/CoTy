using System.Collections.Generic;

namespace CoTy.Objects
{
    public class Assoc : Cobject<Dictionary<Symbol, Cobject>, Assoc>
    {
        public Assoc()
            : base(new Dictionary<Symbol, Cobject>())
        {
        }
    }
}
