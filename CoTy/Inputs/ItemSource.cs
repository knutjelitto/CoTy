using System.Collections.Generic;

namespace CoTy.Inputs
{
    public abstract class ItemSource<TItem>
    {
        private readonly IEnumerable<TItem> itemSource;
        private readonly IEnumerator<TItem> itemIterator;
        private readonly List<TItem> items = new List<TItem>();
        private bool done;

        public ItemSource(IEnumerable<TItem> itemSource)
        {
            this.itemSource = itemSource;
            this.itemIterator = this.itemSource.GetEnumerator();
            this.done = false;
        }

        protected abstract TItem EndOfItems { get; }

        public TItem this[int index]
        {
            get
            {
                Suck(index);
                return index < this.items.Count ? this.items[index] : EndOfItems;
            }
        }

        public bool Has(int index)
        {
            return !this[index].Equals(EndOfItems);
        }

        private void Suck(int index)
        {
            while (!this.done && index >= this.items.Count)
            {
                if (this.itemIterator.MoveNext() && !this.itemIterator.Current.Equals(EndOfItems))
                {
                    this.items.Add(this.itemIterator.Current);
                }
                else
                {
                    this.done = true;
                }
            }
        }
    }
}
