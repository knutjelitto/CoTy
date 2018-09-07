using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Inputs
{
    public class Parser : IEnumerable<Cobject>
    {
        private readonly Scanner Scanner;

        public Parser(Scanner scanner)
        {
            this.Scanner = scanner;
        }

        public IEnumerator<Cobject> GetEnumerator()
        {
            var current = new Cursor<Cobject>(new ObjectSource(this.Scanner));

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (current)
            {
                yield return ParseObject(current);
            }
        }

        private Cobject ParseObject(Cursor<Cobject> current)
        {
            if (Equals(current.Item, Symbol.LeftParent))
            {
                return ParseQuotation(current);
            }

            if (Equals(current.Item, Symbol.RightParent))
            {
                throw new ParserException("unbalanced `)´ in input");
            }

            if (Equals(current.Item, Symbol.Quoter))
            {
                current.Advance();
                if (!current)
                {
                    throw new ParserException($"dangling `{Symbol.Quoter}´ at end of input");
                }

                return new QuotationLiteral(ParseObject(current));
            }

            var @object = current.Item;
            current.Advance();

            return @object;
        }

        private Cobject ParseQuotation(Cursor<Cobject> current)
        {
            Debug.Assert(Equals(current.Item, Symbol.LeftParent));
            current.Advance();

            IEnumerable<Cobject> Loop()
            {
                while (current && !Equals(current.Item, Symbol.RightParent))
                {
                    yield return ParseObject(current);
                }
            }

            var quotation = new QuotationLiteral(Loop().ToList());

            if (!current)
            {
                throw new ParserException("unbalanced `(´ in input");
            }

            Debug.Assert(Equals(current.Item, Symbol.RightParent));
            current.Advance();

            return quotation;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
