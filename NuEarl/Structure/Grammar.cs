using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuEarl.Support;

namespace NuEarl.Structure
{
    public abstract partial class Grammar
    {
        private ListSet<Symbol> Symbols { get; } = new ListSet<Symbol>();

        public IEnumerable<Nonterminal> Nonterminals => Symbols.OfType<Nonterminal>();
        public IEnumerable<Terminal> Terminals => Symbols.OfType<Terminal>();

        public List<Rule> Rules { get; } = new List<Rule>();

        public Nonterminal Start { get; protected set; }
    }
}
