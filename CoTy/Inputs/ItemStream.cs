using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace CoTy.Inputs
{
    public abstract class ItemStream<T> : IEnumerable<T>
    {
        public abstract IEnumerator<T> GetEnumerator();

        protected virtual void OpenLevel()
        {
        }

        protected virtual void CloseLevel()
        {
        }

        public virtual IDisposable LevelUp()
        {
            OpenLevel();
            return Disposable.Create(CloseLevel);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
