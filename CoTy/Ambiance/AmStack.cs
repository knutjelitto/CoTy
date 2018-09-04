using System;
using System.Collections.Generic;
using System.Linq;

using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class AmStack
    {
        private readonly Stack<CoTuple> stack = new Stack<CoTuple>();

        public void Push(CoTuple obj)
        {
            this.stack.Push(obj);
        }

        public CoTuple Pop()
        {
            return this.stack.Pop();
        }

        public T Pop<T>() where T : CoTuple
        {
            return (T)this.stack.Pop();
        }

        public CoTuple Peek()
        {
            return this.stack.Peek();
        }

        public int Count => this.stack.Count;

        public void Clear()
        {
            this.stack.Clear();
        }

        public void Dump()
        {
            var text = string.Join(" ", this.stack.Reverse());
            Console.WriteLine($"[{text}]");
        }
    }
}
