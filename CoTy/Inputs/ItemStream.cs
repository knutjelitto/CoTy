using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Inputs
{
    public abstract class ItemStream<T> : IEnumerable<T>
    {
        protected ItemStream()
        {
        }

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
