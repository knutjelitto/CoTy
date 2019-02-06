using System.Collections.Generic;
using System.Linq;

namespace NuEarl.Structure
{
    public partial class Nonterminal : Symbol
    {
        public Nonterminal(Grammar grammar, string name) : base(grammar, name)
        {
        }

        public List<Rule> Rules { get; } = new List<Rule>();

        protected Nonterminal AddRule(Symbols symbols)
        {
            Rules.Add(new Rule(this, symbols));

            return this;
        }
    }
}
