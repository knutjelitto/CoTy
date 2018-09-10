using System;
using System.Collections.Generic;
using System.Linq;
using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class AmStack
    {
        private readonly Stack<Cobject> stack = new Stack<Cobject>();

        public void Push(Cobject obj)
        {
            this.stack.Push(obj);
        }

        public Cobject Pop()
        {
            if (this.stack.Count == 0)
            {
                throw new StackException(1, 0);
            }
            return this.stack.Pop();
        }

        public Cobject Peek()
        {
            if (this.stack.Count == 0)
            {
                throw new StackException(1, 0);
            }
            return this.stack.Peek();
        }

        public int Count => this.stack.Count;

        public void Clear()
        {
            this.stack.Clear();
        }

        public Sequence Get()
        {
            return new Sequence(this.stack.Reverse().ToList());
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
