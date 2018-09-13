using System.Collections;
using System.Collections.Generic;

namespace CoTy.Inputs
{
    public abstract class ItemStream<T> : IEnumerable<T>
    {
        public abstract IEnumerator<T> GetEnumerator();

        public virtual void OpenLevel()
        {
        }

        public virtual void CloseLevel()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
