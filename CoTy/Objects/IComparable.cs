using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public interface IComparable<T> where T : CoTuple
    {
        Bool Equal(T other);
    }
}
