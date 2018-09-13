using System;
using System.Collections.Generic;
using System.Linq;
using CoTy.Errors;

namespace CoTy.Objects
{
    public class Stack : Cobject<Stack<Cobject>>
    {
        public Stack() : base(new Stack<Cobject>())
        {
        }

        public void Push(Cobject obj)
        {
            Value.Push(obj);
        }

        public Cobject Pop()
        {
            if (Value.Count == 0)
            {
                throw new StackException(1, 0);
            }
            return Value.Pop();
        }

        public Cobject Peek()
        {
            if (Value.Count == 0)
            {
                throw new StackException(1, 0);
            }
            return Value.Peek();
        }

        public void Clear()
        {
            Value.Clear();
        }

        public Sequence Get()
        {
            return Sequence.From(Value.Reverse().ToList());
        }

        public void Dump()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return $"<{string.Join(" ", Get().Select(item => item.ToString() + ":" + item.GetType().Name))}>";
            //return $"<{string.Join(" ", Get())}>";
        }
    }
}
