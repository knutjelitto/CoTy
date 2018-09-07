using System;
using System.Collections.Generic;
using System.Text;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public interface IStack
    {
        Cobject Pop();
        Cobject Peek();
        void Push(Cobject value);
        int Count { get; }
        void Clear();
    }
}
