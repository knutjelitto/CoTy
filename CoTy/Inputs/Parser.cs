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
            if (TryParseSequence(current, out var result))
            {
                return result;
            }

            if (TryParseBinder(current, out var binder))
            {
                return binder;
            }

            if (TryParseAssigner(current, out var assigner))
            {
                return assigner;
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

        private bool TryParseBinder(Cursor<Cobject> current, out Binder binder)
        {
            if (!Equals(current.Item, Symbol.Bind))
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
                        throw new ParserException($"binder objects sequence `{sequence}´ should contain at least one symbol");
                    }
                    if (!sequence.AllSymbols())
                    {
                        throw new ParserException($"binder objects sequence `{sequence}´ should only contain symbols");
                    }

                    binder = Binder.From(sequence.Cast<Symbol>());
                }
                else
                {
                    if (!(current.Item is Symbol symbol))
                    {
                        throw new ParserException($"binder object `{current.Item}´ should be a symbols");
                    }

                    binder = Binder.From(symbol);

                    current.Advance();
                }

                return true;
            }
        }


        private bool TryParseAssigner(Cursor<Cobject> current, out Assigner assigner)
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
                        throw new ParserException($"binder object `{current.Item}´ should be a symbols");
                    }

                    assigner = Assigner.From(symbol);

                    current.Advance();
                }

                return true;
            }
        }

        private bool TryParseSequence(Cursor<Cobject> current, out Sequence sequence)
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

                sequence = Sequence.From(Loop().ToList());

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
