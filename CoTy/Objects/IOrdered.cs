using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public interface IOrdered<T> where T : Cobject
    {
        Bool Less(T other);
    }
}
