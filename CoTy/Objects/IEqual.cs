using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public interface IEqual<T>
    {
        Bool Equal(T other);
    }
}
