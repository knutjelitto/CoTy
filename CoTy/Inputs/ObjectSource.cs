using System.Collections.Generic;

using CoTy.Objects;

namespace CoTy.Inputs
{
    public class ObjectSource : ItemSource<Cobject>
    {
        public ObjectSource(IEnumerable<Cobject> objectSource)
            : base(objectSource)
        {
        }

        protected override Cobject EndOfItems => Symbol.End;
    }
}
