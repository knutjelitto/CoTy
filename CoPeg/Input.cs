using System.Collections.Generic;
using System.Linq;

namespace CoPeg
{
    public abstract class Input<T> : IInput<T>
    {
        private readonly List<T> elements;

        public Input(IEnumerable<T> elements)
        {
            this.elements = elements.ToList();
        }

        public ISequence<T> First => new Sequencer(this, 0);

        private class Sequencer : ISequence<T>
        {
            private readonly Input<T> input;
            private readonly int index;

            public Sequencer(Input<T> input, int index)
            {
                this.input = input;
                this.index = index;
            }

            public T Current => this.input.elements[this.index];

            public ISequence<T> Next()
            {
                return new Sequencer(this.input, this.index+1);
            }
        }
    }
}
