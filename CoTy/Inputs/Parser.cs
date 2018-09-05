using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Inputs
{
    public class Parser : IEnumerable<Cobject>
    {
        public readonly Scanner Scanner;

        public Parser(Scanner scanner)
        {
            this.Scanner = scanner;
        }

        public IEnumerator<Cobject> GetEnumerator()
        {
            var current = new Cursor<Cobject>(new ObjectSource(this.Scanner));
            var queue = new Queue<Cobject>(2);

            while (current)
            {
                ParseObject(queue, ref current);
                while (queue.TryDequeue(out var obj))
                {
                    yield return obj;
                }
            }
        }

        private void ParseObject(Queue<Cobject> queue, ref Cursor<Cobject> current)
        {
            if (current.Item is Symbol symbol)
            {
                if (Equals(symbol, Symbol.LeftParent))
                {
                    ParseQuotation(queue, ref current);
                    return;
                }
                if (Equals(symbol, Symbol.RightParent))
                {
                    throw new ParserException("ill: unbalanced ')' in input");
                }

                if (Equals(symbol, Symbol.Quoter))
                {
                    current = current.Next;
                    if (!current)
                    {
                        throw new ParserException($"ill: dangling {Symbol.Quoter} at end of input");
                    }
                    ParseObject(queue, ref current);
                    var quotation = new QuotationLiteral(queue.ToList());
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

        private void ParseQuotation(Queue<Cobject> queue, ref Cursor<Cobject> current)
        {
            current = current.Next;
            var inner = new Queue<Cobject>();

            while (current && !Equals(current.Item, Symbol.RightParent))
            {
                ParseObject(inner, ref current);
            }
            if (!current)
            {
                throw new ParserException("ill: unbalanced '(' in input");
            }

            queue.Enqueue(new QuotationLiteral(inner));
            current = current.Next;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
