using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

using CoTy.Objects;
using CoTy.Errors;

namespace CoTy.Inputs
{
    public class Scanner : IEnumerable<CoObject>
    {
        private readonly CharSource source;

        public Scanner(CharSource source)
        {
            this.source = source;
        }

        public IEnumerator<CoObject> GetEnumerator()
        {
            var current = new Cursor<char>(this.source);

            current = Skip(current);

            while (current)
            {
                switch (current)
                {
                    case '(':
                        current = current.Next;
                        yield return CoSymbol.LeftParent;
                        break;
                    case ')':
                        current = current.Next;
                        yield return CoSymbol.RightParent;
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

        private bool IsDigit(char c)
        {
            return '0' <= c && c <= '9';
        }

        private bool IsSkipable(char c)
        {
            return char.IsWhiteSpace(c) || char.IsControl(c);
        }

        private bool IsStructure(char c)
        {
            return "()".Contains(c);
        }

        private Cursor<char> Skip(Cursor<char> current)
        {
            while (current && IsSkipable(current))
            {
                current = current.Next;
            }
            return current;
        }

        private CoString ScanString(ref Cursor<char> current)
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
            return new CoString(accu.ToString());
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
            while (current && !IsSkipable(current) && !IsStructure(current));

            return accu.ToString();
        }

        private CoObject Classify(string grumble)
        {
            if (BigInteger.TryParse(grumble, out var integer))
            {
                return new CoInteger(integer);
            }
            return CoSymbol.Get(grumble);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
