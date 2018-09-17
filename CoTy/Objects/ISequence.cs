using System.Collections.Generic;

namespace CoTy.Objects
{
    public interface ISequence
    {
        IEnumerable<object> GetIterator();
    }
}
