using System;
using System.Collections.Generic;
using System.Linq;
using CoTy.Errors;

namespace CoTy.Objects
{
    public class Stack : Cobject<List<object>>
    {
        public Stack() : base(new List<object>())
        {
        }

        public void Push(object value)
        {
            Value.Add(value);
        }

        public object Pop()
        {
            if (Value.Count == 0)
            {
                throw new StackException(1, 0);
            }

            var popped = Value[Value.Count - 1];
            Value.RemoveAt(Value.Count - 1);
            return popped;
        }

        public object Peek()
        {
            if (Value.Count == 0)
            {
                throw new StackException(1, 0);
            }
            return Value[Value.Count - 1];
        }

        public void Clear()
        {
            Value.Clear();
        }

        public Sequence Get()
        {
            return Sequence.From(Value.ToList());
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
