using System;
using System.Collections.Generic;

namespace NuEarl.Structure
{
    partial class Nonterminal : Symbol
    {
        public static Nonterminal operator |(Nonterminal define, Symbols symbols)
        {
            return define.AddRule(symbols);
        }

        public static Nonterminal operator |(Nonterminal define, Symbol symbol)
        {
            return define.AddRule(new Symbols { symbol });
        }

        public static Nonterminal operator |(Nonterminal define, string literal)
        {
            return define.AddRule(new Symbols { define.Grammar.Literal(literal) });
        }

        public static Symbols operator +(Nonterminal sym, string literal)
        {
            return new Symbols {sym, sym.Grammar.Literal(literal)};
        }

        public static Symbols operator +(string literal, Nonterminal sym)
        {
            return new Symbols {sym.Grammar.Literal(literal), sym};
        }

        public static Symbols operator +(Nonterminal sym1, Nonterminal sym2)
        {
            return new Symbols {sym1, sym2};
        }

        public static Symbols operator +(Symbols symbols, Nonterminal sym2)
        {
            return new Symbols(symbols) {sym2};
        }
    }
}
