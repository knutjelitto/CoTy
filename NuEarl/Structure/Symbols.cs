using System;
using System.Collections.Generic;
using System.Text;
using NuEarl.Support;

namespace NuEarl.Structure
{
    public partial class Symbols : List<Symbol>
    {
        public Symbols(params Symbol[] syms)
        {
            AddRange(syms);
        }

        public Symbols(Symbols symbols)
        {
            AddRange(symbols);
        }

        public override string ToString()
        {
            return string.Join(" ", this);
        }
    }
}
