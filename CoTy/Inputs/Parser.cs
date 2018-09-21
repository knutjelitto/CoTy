using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Inputs
{
    public class Parser : ItemStream<object>
    {
        private Scanner Scanner { get; }

        public Parser(ItemStream<char> charStream)
        {
            var source = new ItemSource<char>(charStream);
            Scanner = new Scanner(source);
        }

        public override IEnumerator<object> GetEnumerator()
        {
            var current = new Cursor<object>(new ItemSource<object>(Scanner));

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (current)
            {
                yield return ParseObject(current);
            }
        }

        private object ParseObject(Cursor<object> current)
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

            if (Equals(current.Item, Symbol.Quoter))
            {
                current.Advance();
                if (!current)
                {
                    throw new ParserException($"dangling `{Symbol.Quoter}´ at end of input");
                }

                return BlockLiteral.From(Enumerable.Repeat(ParseObject(current), 1));
            }

            if (Equals(current.Item, Symbol.RightParent))
            {
                throw new ParserException($"unbalanced `{Symbol.RightParent}´ in input");
            }

            var @object = current.Item;
            current.Advance();

            return @object;
        }

        private bool TryParseDefiner(Cursor<object> current, out Definer binder)
        {
            if (!Equals(current.Item, Symbol.BindTo))
            {
                binder = null;
                return false;
            }

            using (Scanner.LevelUp())
            {
                current.Advance();

                if (TryParseSequence(current, out var sequence))
                {
                    if (sequence.IsEmpty())
                    {
                        throw new ParserException($"definer objects sequence `{sequence}´ should contain at least one symbol");
                    }
                    if (!sequence.AllSymbols())
                    {
                        throw new ParserException($"definer objects sequence `{sequence}´ should only contain symbols");
                    }

                    binder = Definer.From(sequence.Cast<Symbol>());
                }
                else
                {
                    if (!(current.Item is Symbol symbol))
                    {
                        throw new ParserException($"definer object `{current.Item}´ should be a symbol");
                    }

                    binder = Definer.From(symbol);

                    current.Advance();
                }

                return true;
            }
        }


        private bool TryParseAssigner(Cursor<object> current, out Assigner assigner)
        {
            if (!Equals(current.Item, Symbol.Assign))
            {
                assigner = null;
                return false;
            }

            using (Scanner.LevelUp())
            {
                current.Advance();

                if (TryParseSequence(current, out var sequence))
                {
                    if (sequence.IsEmpty())
                    {
                        throw new ParserException($"assigner objects sequence `{sequence}´ should contain at least one symbol");
                    }
                    if (!sequence.AllSymbols())
                    {
                        throw new ParserException($"assigner objects sequence `{sequence}´ should only contain symbols");
                    }

                    assigner = Assigner.From(sequence.Cast<Symbol>());
                }
                else
                {
                    if (!(current.Item is Symbol symbol))
                    {
                        throw new ParserException($"assigner object `{current.Item}´ should be a symbol");
                    }

                    assigner = Assigner.From(symbol);

                    current.Advance();
                }

                return true;
            }
        }

        private bool TryParseSequence(Cursor<object> current, out BlockLiteral sequence)
        {
            if (!Equals(current.Item, Symbol.LeftParent))
            {
                sequence = null;
                return false;

            }

            using (Scanner.LevelUp())
            {
                current.Advance();

                IEnumerable<object> Loop()
                {
                    while (current && !Equals(current.Item, Symbol.RightParent))
                    {
                        yield return ParseObject(current);
                    }
                }

                sequence = BlockLiteral.From(Loop().ToList());

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
