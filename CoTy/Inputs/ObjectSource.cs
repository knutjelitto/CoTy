using System.Collections.Generic;

using CoTy.Objects;

namespace CoTy.Inputs
{
    public class ObjectSource : ItemSource<CoObject>
    {
        public ObjectSource(IEnumerable<CoObject> objectSource)
            : base(objectSource)
        {
        }

        protected override CoObject EndOfItems => CoSymbol.End;
    }
}
