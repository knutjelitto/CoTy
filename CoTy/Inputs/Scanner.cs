using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using CoTy.Objects;
using CoTy.Errors;
using System;

namespace CoTy.Inputs
{
    public class Scanner : IEnumerable<Cobject>
    {
        private readonly CharSource source;

        public Scanner(CharSource source)
        {
            this.source = source;
        }

        public IEnumerator<Cobject> GetEnumerator()
        {
            var current = new Cursor<char>(this.source);

            current = Skip(current);

            while (current)
            {
                switch (current)
                {
                    case '(':
                        current = current.Next;
                        yield return Symbol.LeftParent;
                        break;
                    case ')':
                        current = current.Next;
                        yield return Symbol.RightParent;
                        break;
                    case '\'':
                        current = current.Next;
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

        private bool IsLineComment(Cursor<char> current)
        {
            return current == ';' && current.Next == ';';
        }

        private Cursor<char> Skip(Cursor<char> current)
        {
            while (current)
            {
                while (current && IsSkipable(current))
                {
                    current = current.Next;
                }

                if (current && current == ';' && current.Next == ';')
                {
                    while (current && current != CharSource.NL)
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

        private Cursor<char> SkipLineComment(Cursor<char> current)
        {
            while (current && current != CharSource.NL)
            {
                current = current.Next;
            }
            return current;
        }

        private Chars ScanString(ref Cursor<char> current)
        { 
            Debug.Assert(current == '"');

            var accu = new StringBuilder();

            current = current.Next;
            while (current && current != '"')
            {
                if (current == '\\' && current.Next == '"')
                {
                    accu.Append('"');
                    current = current.Next;
                }
                else
                {
                    accu.Append((char)current);
                }
                current = current.Next;
            }
            if (current != '"')
            {
                throw new ScannerException("EOT in string literal");
            }
            current = current.Next;
            return new Chars(accu.ToString());
        }

        private string ScanGrumble(ref Cursor<char> current)
        {
            Debug.Assert(current && !IsSkipable(current) && !IsStructure(current));

            var accu = new StringBuilder();

            do
            {
                accu.Append((char)current);
                current = current.Next;
            }
            while (current && !IsSkipable(current) && !IsStructure(current) && !IsLineComment(current));

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
