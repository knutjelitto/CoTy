using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NuEarl.Support
{
    public class ListSet<T> : ICollection<T>
    {
        private readonly List<T> list = new List<T>();
        private readonly HashSet<T> set = new HashSet<T>();

        public IEnumerator<T> GetEnumerator() => this.list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            if (!this.set.Contains(item))
            {
                this.list.Add(item);
                this.set.Add(item);
            }
        }

        public void Clear()
        {
            this.list.Clear();
            this.set.Clear();
        }

        public bool Contains(T item) => this.set.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            this.list.Remove(item);
            return this.set.Remove(item);
        }

        public int Count => this.list.Count;
        public bool IsReadOnly => false;
    }
}
