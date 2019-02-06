using System;
using System.Collections.Generic;
using System.Text;

namespace CoPeg
{
    public interface ISequence<T>
    {
        T Current { get; }

        ISequence<T> Next();
    }
}
