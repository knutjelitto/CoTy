using System.Collections.Generic;

namespace CoTy.Objects
{
    public class Assoc : Cobject<Dictionary<Symbol, Cobject>>
    {
        public Assoc()
            : base(new Dictionary<Symbol, Cobject>())
        {
        }
    }
}
