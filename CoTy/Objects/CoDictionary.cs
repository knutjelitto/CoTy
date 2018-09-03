using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public class CoDictionary : CoObject<Dictionary<CoSymbol, CoObject>>
    {
        public CoDictionary()
            : base(new Dictionary<CoSymbol, CoObject>())
        {
        }
    }
}
