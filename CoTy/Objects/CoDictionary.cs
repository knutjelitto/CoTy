using System.Collections.Generic;

namespace CoTy.Objects
{
    public class CoDictionary : CoTuple<Dictionary<CoSymbol, CoTuple>>
    {
        public CoDictionary()
            : base(new Dictionary<CoSymbol, CoTuple>())
        {
        }
    }
}
