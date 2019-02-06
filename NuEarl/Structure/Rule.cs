using System.Collections.Generic;
using System.Diagnostics;

namespace NuEarl.Structure
{
    public class Rule
    {
        public Rule(Nonterminal name, Symbols symbols)
        {
            Debug.Assert(name != null);
            Debug.Assert(symbols != null);

            Name = name;
            Symbols = symbols;
        }

        public Nonterminal Name { get; }

        public Symbols Symbols { get; }

        public override string ToString()
        {
            return base.ToString(); 
        }
    }
}
