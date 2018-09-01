using System;
using System.Collections;
using System.Collections.Generic;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Inputs
{
    public class Parser : IEnumerable<CoObject>
    {
        private readonly Scanner scanner;

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
        }

        public IEnumerator<CoObject> GetEnumerator()
        {
            var current = new Cursor<CoObject>(new ObjectSource(this.scanner));
            var queue = new Queue<CoObject>(2);

            while (current)
            {
                ParseObjects(queue, ref current);
                while (queue.TryDequeue(out var obj))
                {
                    yield return obj;
                }
            }
        }

        private void ParseObjects(Queue<CoObject> queue, ref Cursor<CoObject> current)
        {
            if (current.Item is CoSymbol symbol)
            {
                if (symbol == CoSymbol.LeftParent)
                {
                    ParseQuote(queue, ref current);
                    return;
                }
                if (symbol == CoSymbol.RightParent)
                {
                    throw new ParserException("ill: unbalanced ')' in input");
                }
                if (symbol.Value.Length > 1)
                {
                    switch (symbol.Value[0])
                    {
                        case ':':
                            queue.Enqueue(CoSymbol.Get(symbol.Value.Substring(1)));
                            queue.Enqueue(CoSymbol.Define);
                            current = current.Next;
                            return;
                    }
                }
            }

            queue.Enqueue(current.Item);
            current = current.Next;
        }

        private void ParseQuote(Queue<CoObject> queue, ref Cursor<CoObject> current)
        {
            current = current.Next;
            var inner = new Queue<CoObject>();

            while (current && current.Item != CoSymbol.RightParent)
            {
                ParseObjects(inner, ref current);
            }
            if (!current)
            {
                throw new ParserException("ill: unbalanced '(' in input");
            }

            queue.Enqueue(new CoQuote(inner));
            current = current.Next;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
