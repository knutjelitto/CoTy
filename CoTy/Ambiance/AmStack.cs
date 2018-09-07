using System;
using System.Collections.Generic;
using System.Linq;

using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class AmStack : IStack
    {
        private readonly Stack<Cobject> stack = new Stack<Cobject>();

        public void Push(Cobject obj)
        {
            this.stack.Push(obj);
        }

        public Cobject Pop()
        {
            return this.stack.Pop();
        }

        public Cobject Peek()
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
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            var text = string.Join(" ", this.stack.Reverse());
            return $"[{text}]";
        }
    }
}
