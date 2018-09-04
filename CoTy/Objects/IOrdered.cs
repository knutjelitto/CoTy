using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public interface IOrdered<T> where T : CoTuple
    {
        Bool Less(T other);
    }
}
