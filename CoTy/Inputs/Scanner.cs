using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using CoTy.Objects;
using CoTy.Errors;

namespace CoTy.Inputs
{
    public class Scanner : ItemStream<object>
    {
        private readonly ItemSource<char> source;

        public Scanner(ItemSource<char> source)
        {
            this.source = source;
        }

        public override IDisposable LevelUp()
        {
            return this.source.LevelUp();
        }

        public override IEnumerator<object> GetEnumerator()
        {
            var current = new Cursor<char>(this.source);

            current = Skip(current);

            while (current)
            {
                switch (current.Item)
                {
                    case '(':
                        current.Advance();
                        yield return Symbol.LeftParent;
                        break;
                    case ')':
                        current.Advance();
                        yield return Symbol.RightParent;
                        break;
                    //case '{':
                    //    current.Advance();
                    //    yield return Symbol.LeftBrace;
                    //    break;
                    //case '}':
                    //    current.Advance();
                    //    yield return Symbol.RightBrace;
                    //    break;
                    //case var eq when eq == '=' && current.Next && current.Next.Item == ':':
                    //    current.Advance();
                    //    current.Advance();
                    //    yield return Symbol.Bind;
                    //    break;
                    //case var eq when eq == '=' && current.Next && current.Next.Item == '>':
                    //    current.Advance();
                    //    current.Advance();
                    //    yield return Symbol.Assign;
                    //    break;
                    case '\'':
                        current.Advance();
                        yield return Symbol.Quoter;
                        break;
                    case '"':
                        yield return ScanString(ref current);
                        break;
                    default:
                        yield return Classify(ScanGrumble(ref current));
                        break;
                }

                current = Skip(current);
            }
        }

        private bool IsSkipable(char c)
        {
            return char.IsWhiteSpace(c) || char.IsControl(c);
        }

        private bool IsStructure(char c)
        {
            return "()\"\'".Contains(c);
        }

        private bool MoreToScan(Cursor<char> current)
        {
            return current && !IsSkipable(current.Item) && !IsStructure(current.Item) && !IsLineComment(current);
        }

        private bool IsLineComment(Cursor<char> current)
        {
            return current && current.Item == ';' && current.Next && current.Next.Item == ';';
        }

        private Cursor<char> Skip(Cursor<char> current)
        {
            while (current)
            {
                while (current && IsSkipable(current.Item))
                {
                    current = current.Next;
                }

                if (IsLineComment(current))
                {
                    while (current && current.Item != '\n')
                    {
                        current = current.Next;
                    }
                }
                else
                {
                    break;
                }
            }

            return current;
        }

        private string ScanString(ref Cursor<char> current)
        { 
            Debug.Assert(current.Item == '"');

            var accu = new StringBuilder();

            current = current.Next;
            while (current && current.Item != '"')
            {
                if (current.Item == '\\' && current.Next.Item == '"')
                {
                    accu.Append('"');
                    current = current.Next;
                }
                else
                {
                    accu.Append(current.Item);
                }
                current = current.Next;
            }
            if (current.Item != '"')
            {
                throw new ScannerException("EOT in string literal");
            }
            current = current.Next;

            return accu.ToString();
        }

        private string ScanGrumble(ref Cursor<char> current)
        {
            Debug.Assert(current && !IsSkipable(current.Item) && !IsStructure(current.Item));

            var accu = new StringBuilder();

            do
            {
                accu.Append(current.Item);
                current = current.Next;
            }
            while (MoreToScan(current));

            return accu.ToString();
        }

        private Cobject Classify(string grumble)
        {
            if (Integer.TryFrom(grumble, out var integer))
            {
                return integer;
            }
            return Symbol.Get(grumble);
        }
    }
}
