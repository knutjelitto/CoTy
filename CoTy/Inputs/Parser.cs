using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Inputs
{
    public class Parser : ItemStream<Cobject>
    {
        private readonly Scanner Scanner;

        public Parser(Scanner scanner)
        {
            this.Scanner = scanner;
        }

        public override IEnumerator<Cobject> GetEnumerator()
        {
            var current = new Cursor<Cobject>(new ItemSource<Cobject>(this.Scanner));

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

                return Sequence.From(ParseObject(current));
            }

            var @object = current.Item;
            current.Advance();

            return @object;
        }

        private Cobject ParseQuotation(Cursor<Cobject> current)
        {
            Debug.Assert(Equals(current.Item, Symbol.LeftParent));
            this.Scanner.OpenLevel();
            current.Advance();

            IEnumerable<Cobject> Loop()
            {
                while (current && !Equals(current.Item, Symbol.RightParent))
                {
                    yield return ParseObject(current);
                }
            }

            var quotation = Sequence.From(Loop().ToList());

            if (!current)
            {
                throw new ParserException("unbalanced `(´ in input");
            }

            Debug.Assert(Equals(current.Item, Symbol.RightParent));
            current.Advance();
            this.Scanner.CloseLevel();

            return quotation;
        }
    }
}
