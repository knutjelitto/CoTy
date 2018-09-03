using System;
using System.Collections.Generic;
using System.Linq;

using CoTy.Objects;

namespace CoTy.Ambiance
{
    public class AmStack
    {
        private readonly Stack<CoObject> stack = new Stack<CoObject>();

        public void Push(CoObject obj)
        {
            stack.Push(obj);
        }

        public dynamic Pop()
        {
            return stack.Pop();
        }

        public CoObject Peek()
        {
            return stack.Peek();
        }

        public void Clear()
        {
            stack.Clear();
        }

        public void Dump()
        {
            var stack = string.Join(" ", this.stack.Reverse());
            Console.WriteLine($"[{stack}]");
        }
    }
}
