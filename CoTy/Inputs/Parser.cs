using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Inputs
{
    public class Parser : IEnumerable<CoTuple>
    {
        public readonly Scanner Scanner;

        public Parser(Scanner scanner)
        {
            this.Scanner = scanner;
        }

        public IEnumerator<CoTuple> GetEnumerator()
        {
            var current = new Cursor<CoTuple>(new ObjectSource(this.Scanner));
            var queue = new Queue<CoTuple>(2);

            while (current)
            {
                ParseObject(queue, ref current);
                while (queue.TryDequeue(out var obj))
                {
                    yield return obj;
                }
            }
        }

        private void ParseObject(Queue<CoTuple> queue, ref Cursor<CoTuple> current)
        {
            if (current.Item is Symbol symbol)
            {
                if (symbol == Symbol.LeftParent)
                {
                    ParseQuote(queue, ref current);
                    return;
                }
                if (symbol == Symbol.RightParent)
                {
                    throw new ParserException("ill: unbalanced ')' in input");
                }

                if (symbol == Symbol.Quoter)
                {
                    current = current.Next;
                    if (!current)
                    {
                        throw new ParserException($"ill: dangling {Symbol.Quoter} at end of input");
                    }
                    ParseObject(queue, ref current);
                    var quotation = new Quotation(queue.ToList());
                    queue.Clear();
                    queue.Enqueue(quotation);
                    return;
                }
                if (symbol.Value.Length > 1)
                {
                    switch (symbol.Value[0])
                    {
                        case ':':
                            queue.Enqueue(new Chars(symbol.Value.Substring(1)));
                            queue.Enqueue(Symbol.Define);
                            current = current.Next;
                            return;
                    }
                }
            }

            queue.Enqueue(current.Item);
            current = current.Next;
        }

        private void ParseQuote(Queue<CoTuple> queue, ref Cursor<CoTuple> current)
        {
            current = current.Next;
            var inner = new Queue<CoTuple>();

            while (current && current.Item != Symbol.RightParent)
            {
                ParseObject(inner, ref current);
            }
            if (!current)
            {
                throw new ParserException("ill: unbalanced '(' in input");
            }

            queue.Enqueue(new Quotation(inner));
            current = current.Next;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
