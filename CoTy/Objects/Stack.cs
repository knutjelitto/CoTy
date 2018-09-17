using System;
using System.Collections.Generic;
using System.Linq;

using CoTy.Errors;

// ReSharper disable MemberCanBePrivate.Global
namespace CoTy.Objects
{
    public class Stack : Cobject<List<object>>, ISequence
    {
        private Stack(IEnumerable<object> values) : base(new List<object>(values))
        {
        }

        public static Stack From(IEnumerable<object> values)
        {
            return new Stack(values);
        }

        public static Stack From(params object[] values)
        {
            return From(values.AsEnumerable());
        }

        public int Count => Value.Count;

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

        public void Dup()
        {
            if (Value.Count == 0)
            {
                throw new StackException(1, 0);
            }

            Value.Add(Value[Value.Count - 1]);
        }

        public void Drop()
        {
            if (Value.Count == 0)
            {
                throw new StackException(1, 0);
            }

            Value.RemoveAt(Value.Count - 1);
        }

        public void Swap()
        {
            if (Value.Count < 2)
            {
                throw new StackException(2, Value.Count);
            }

            var tmp = Value[Value.Count - 2];
            Value[Value.Count - 2] = Value[Value.Count - 1];
            Value[Value.Count - 1] = tmp;
        }

        public void Over()
        {
            if (Value.Count < 2)
            {
                throw new StackException(2, Value.Count);
            }

            Value.Add(Value[Value.Count - 2]);
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

        public IEnumerable<object> GetIterator()
        {
            return Value;
        }
    }
}
