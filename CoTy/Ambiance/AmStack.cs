using System;
using System.Collections.Generic;

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

        public CoObject Pop()
        {
            return stack.Pop();
        }

        public void Dump()
        {
            foreach (var @object in this.stack)
            {
                Console.WriteLine(@object);
            }
        }
    }
}
