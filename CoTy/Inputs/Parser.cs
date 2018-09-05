using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CoTy.Ambiance;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Inputs
{
    public class Parser
    {
        private readonly Scanner Scanner;

        public Parser(Scanner scanner)
        {
            this.Scanner = scanner;
        }

        public IEnumerable<Cobject> Parse(AmScope lexical)
        {
            var current = new Cursor<Cobject>(new ObjectSource(this.Scanner));

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (current)
            {
                yield return ParseObject(lexical, current);
            }
        }

        private Cobject ParseObject(AmScope lexical, Cursor<Cobject> current)
        {
            if (Equals(current.Item, Symbol.LeftParent))
            {
                return ParseQuotation(lexical, current);
            }

            if (Equals(current.Item, Symbol.RightParent))
            {
                throw new ParserException("ill: unbalanced ')' in input");
            }

            if (Equals(current.Item, Symbol.Quoter))
            {
                current.Advance();
                if (!current)
                {
                    throw new ParserException($"ill: dangling {Symbol.Quoter} at end of input");
                }

                return new Quotation(lexical, ParseObject(lexical, current));
            }

            var @object = current.Item;
            current.Advance();

            return @object;
        }

        private Cobject ParseQuotation(AmScope lexical, Cursor<Cobject> current)
        {
            Debug.Assert(Equals(current.Item, Symbol.LeftParent));
            current.Advance();

            lexical = new AmScope(lexical, "lexical");

            IEnumerable<Cobject> Loop(AmScope lex)
            {
                while (current && !Equals(current.Item, Symbol.RightParent))
                {
                    foreach (var @object in ParseObject(lex, current))
                    {
                        yield return @object;
                    }
                }
            }

            var quotation = new Quotation(lexical, Loop(lexical).ToList());

            if (!current)
            {
                throw new ParserException("ill: unbalanced '(' in input");
            }

            Debug.Assert(Equals(current.Item, Symbol.RightParent));
            current.Advance();

            return quotation;
        }
    }
}
