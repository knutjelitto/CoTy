using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public interface IComparable<T> where T : Cobject
    {
        Bool Equal(T other);
    }
}
