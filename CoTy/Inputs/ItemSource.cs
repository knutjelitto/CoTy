using System.Collections.Generic;

namespace CoTy.Inputs
{
    public class ItemSource<TItem>
    {
        private readonly ItemStream<TItem> itemStream;
        private readonly IEnumerator<TItem> itemIterator;
        private readonly List<TItem> items = new List<TItem>();
        private bool done;

        public ItemSource(ItemStream<TItem> itemStream)
        {
            this.itemStream = itemStream;
            this.itemIterator = this.itemStream.GetEnumerator();
            this.done = false;
        }

        public void OpenLevel()
        {
            this.itemStream.OpenLevel();
        }

        public void CloseLevel()
        {
            this.itemStream.CloseLevel();
        }

        public TItem this[int index]
        {
            get
            {
                Suck(index);
                return this.items[index];
            }
        }

        public bool Has(int index)
        {
            Suck(index);
            return index < this.items.Count;
        }

        private void Suck(int index)
        {
            while (!this.done && index >= this.items.Count)
            {
                if (this.itemIterator.MoveNext())
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
