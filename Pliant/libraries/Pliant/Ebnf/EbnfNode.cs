using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pliant.Ebnf
{
    public abstract class EbnfNode
    {
        protected EbnfNode()
        {
        }

        protected bool IsOfType<T>(object obj, out T other) where T : EbnfNode
        {
            if (obj is T that && GetType() == that.GetType())
            {
                other = that;
                return true;
            }

            other = null;
            return false;
        }
    }
}
