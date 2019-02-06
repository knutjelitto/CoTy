using System;

namespace NuEarl.Structure
{
    partial class Grammar
    {
        protected Nonterminal Nonterminal(string name) => Define(new Nonterminal(this, name));

        protected Terminal Terminal(string name) => Define(new Terminal(this, name));

        public Terminal Literal(string nameAndValue)
        {
            var terminal = new Terminal(this, nameAndValue);

            Symbols.Add(terminal);

            return terminal;
        }

        private T Define<T>(T symbol) where T : Symbol
        {
            CheckIsUndefined(symbol);

            Symbols.Add(symbol);

            return symbol;
        }

        private void CheckIsUndefined(Symbol symbol)
        {
            if (Symbols.Contains(symbol))
            {
                throw new Exception($"`{symbol.Name}´ already defined as symbol");
            }
        }
    }
}
