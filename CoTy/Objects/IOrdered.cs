using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public interface IOrdered<in T> where T : Cobject
    {
        // ReSharper disable once UnusedMember.Global
        Bool Less(T other);
    }
}
