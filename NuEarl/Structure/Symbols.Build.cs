using System;
using System.Collections.Generic;
using System.Text;

namespace NuEarl.Structure
{
    partial class Symbols
    {
        public static Symbols operator +(Symbols symbols, string literal)
        {
            return new Symbols(symbols) { symbols[0].Grammar.Literal(literal) };
        }
    }
}
