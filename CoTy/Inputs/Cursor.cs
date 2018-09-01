namespace CoTy.Inputs
{
    public class Cursor<T>
    {
        private readonly ItemSource<T> source;
        private readonly int index;

        public Cursor(ItemSource<T> source)
            : this(source, 0)
        {
        }

        private Cursor(ItemSource<T> source, int index)
        {
            this.source = source;
            this.index = index;
        }

        public Cursor<T> Next => new Cursor<T>(this.source, this.index + 1);

        public T Item => this.source[this.index];

        public static implicit operator bool(Cursor<T> input)
        {
            return input.source.Has(input.index);
        }

        public static implicit operator T(Cursor<T> input)
        {
            return input.source[input.index];
        }
    }
}
