using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public partial class Character
    {
        public Bool Less(Character other)
        {
            return Value < other.Value;
        }

        public Character Succ()
        {
            return new Character((char)(Value + 1));
        }

        public Character Pred()
        {
            return new Character((char)(Value - 1));
        }

    }
}
