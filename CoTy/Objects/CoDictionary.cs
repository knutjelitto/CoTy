using System.Collections.Generic;

namespace CoTy.Objects
{
    public class CoDictionary : CoTuple<Dictionary<Symbol, CoTuple>>
    {
        public CoDictionary()
            : base(new Dictionary<Symbol, CoTuple>())
        {
        }
    }
}
