using System;
using System.Collections.Generic;
using System.Linq;
using CoTy.Errors;

namespace CoTy.Objects
{
    public class Stack : Cobject<Stack<Cobject>, Stack>
    {
        //private readonly Stack<Cobject> stack = new Stack<Cobject>();

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

        public (Cobject x, Cobject y) Pop2()
        {
            if (Count < 2)
            {
                throw new StackException(2, Count);
            }

            var y = Pop();
            var x = Pop();

            return (x, y);
        }

        public dynamic Popd()
        {
            return Pop();
        }

        public (dynamic x, dynamic y) Pop2d()
        {
            if (Count < 2)
            {
                throw new StackException(2, Count);
            }

            var y = Pop();
            var x = Pop();

            return (x, y);
        }

        public T Pop<T>() where T : Cobject
        {
            return (T)Pop();
        }


        public int Count => Value.Count;

        public void Clear()
        {
            Value.Clear();
        }

        public Sequence Get()
        {
            return new Sequence(Value.Reverse().ToList());
        }

        public void Dump()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            var text = string.Join(" ", Get());
            return $"[{text}]";
        }
    }
}
