using System.Collections.Generic;

using CoTy.Objects;

namespace CoTy.Inputs
{
    public class ObjectSource : ItemSource<CoTuple>
    {
        public ObjectSource(IEnumerable<CoTuple> objectSource)
            : base(objectSource)
        {
        }

        protected override CoTuple EndOfItems => CoSymbol.End;
    }
}
