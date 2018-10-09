using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Inputs
{
    public class Parser : ItemStream<Cobject>
    {
        private Scanner Scanner { get; }

        public Parser(ItemStream<char> charStream)
        {
            var source = new ItemSource<char>(charStream);
            Scanner = new Scanner(source);
        }

        public override IEnumerator<Cobject> GetEnumerator()
        {
            var current = new Cursor<Cobject>(new ItemSource<Cobject>(Scanner));

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (current)
            {
                yield return ParseObject(current);
            }
        }

        private Cobject ParseObject(Cursor<Cobject> current)
        {
            if (TryParseSequence(current, out var sequence))
            {
                return sequence;
            }

            if (TryParseDefiner(current, out var binder))
            {
                return binder;
            }

            if (TryParseAssigner(current, out var assigner))
            {
                return assigner;
            }

            if (current.Item is Symbol symbol && current.Next && Equals(current.Next.Item, Symbol.ToBind))
            {
                current.Advance();  // symbol
                current.Advance();  // ':='

                var value = ParseObject(current);

                return Special.Define(symbol, value);
            }

            if (Equals(current.Item, Symbol.Quoter))
            {
                current.Advance();
                if (!current)
                {
                    throw new ParserException($"dangling `{Symbol.Quoter}´ at end of input");
                }

                return SequenceLiteral.Quote(ParseObject(current));
            }

            if (Equals(current.Item, Symbol.RightParent))
            {
                throw new ParserException($"unbalanced `{Symbol.RightParent}´ in input");
            }

            var @object = current.Item;
            current.Advance();

            return @object;
        }

        private bool TryParseDefiner(Cursor<Cobject> current, out Special definer)
        {
            if (!Equals(current.Item, Symbol.BindTo))
            {
                definer = null;
                return false;
            }

            using (Scanner.LevelUp())
            {
                current.Advance();

                if (TryParseSymbolSequence(current, out var sequence))
                {
                    definer = Special.MultiDefine(sequence);
                }
                else
                {
                    definer = Special.SingleDefine(ParseSingleSymbol(current));
                }

                return true;
            }
        }


        private bool TryParseAssigner(Cursor<Cobject> current, out Special assigner)
        {
            if (!Equals(current.Item, Symbol.Assign))
            {
                assigner = null;
                return false;
            }

            using (Scanner.LevelUp())
            {
                current.Advance();

                if (TryParseSymbolSequence(current, out var sequence))
                {
                    assigner =  Special.MultiAssign(sequence);
                }
                else
                {
                    assigner = Special.MultiAssign(ParseSingleSymbol(current));
                }

                return true;
            }
        }

        private bool TryParseSymbolSequence(Cursor<Cobject> current, out SequenceLiteral sequence)
        {
            if (TryParseSequence(current, out sequence))
            {
                if (sequence.IsEmpty())
                {
                    throw new ParserException($"objects sequence `{sequence}´ should contain at least one symbol");
                }
                if (!sequence.AllSymbols())
                {
                    throw new ParserException($"objects sequence `{sequence}´ should only contain symbols");
                }

                return true;
            }

            sequence = null;
            return false;
        }

        private Symbol ParseSingleSymbol(Cursor<Cobject> current)
        {
            if (!(current.Item is Symbol symbol))
            {
                throw new ParserException($"current `{current.Item}´ should be a symbol");
            }

            current.Advance();

            return symbol;
        }

        private bool TryParseSequence(Cursor<Cobject> current, out SequenceLiteral sequence)
        {
            if (!Equals(current.Item, Symbol.LeftParent))
            {
                sequence = null;
                return false;

            }

            using (Scanner.LevelUp())
            {
                current.Advance();

                IEnumerable<Cobject> Loop()
                {
                    while (current && !Equals(current.Item, Symbol.RightParent))
                    {
                        yield return ParseObject(current);
                    }
                }

                sequence = SequenceLiteral.From(Loop().ToList());

                if (!current)
                {
                    throw new ParserException("unbalanced `(´ in input");
                }

                Debug.Assert(Equals(current.Item, Symbol.RightParent));

                current.Advance();

                return true;

            }
        }
    }
}
