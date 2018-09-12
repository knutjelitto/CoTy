using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public interface ICompare<in T> where T : Cobject
    {
        int? Compare(T value1, T value2);
    }
}
