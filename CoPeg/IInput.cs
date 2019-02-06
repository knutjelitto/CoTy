using System;
using System.Collections.Generic;
using System.Text;

namespace CoPeg
{
    public interface IInput<T>
    {
        ISequence<T> First { get; }
    }
}
