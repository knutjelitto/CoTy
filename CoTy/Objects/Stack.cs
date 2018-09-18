using System;
using System.Collections.Generic;
using System.Linq;

using CoTy.Errors;
using CoTy.Support;

// ReSharper disable MemberCanBePrivate.Global
namespace CoTy.Objects
{
    public class Stack : Cobject<List<object>>, IStack
    {
        private Stack(IEnumerable<object> values) : base(new List<object>(values))
        {
        }

        public static IStack From(IEnumerable<object> values)
        {
            return new Stack(values);
        }

        public static IStack From(params object[] values)
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
            Check(1);

            var popped = Value[Value.Count - 1];
            Value.RemoveAt(Value.Count - 1);
            return popped;
        }

        public void Dup()
        {
            Check(1);
            Value.Add(Value[Value.Count - 1]);
        }

        public void Drop()
        {
            Check(1);
            Value.RemoveAt(Value.Count - 1);
        }

        public void Swap()
        {
            Check(2);
            var tmp = Value[Value.Count - 2];
            Value[Value.Count - 2] = Value[Value.Count - 1];
            Value[Value.Count - 1] = tmp;
        }

        public void Over()
        {
            Check(2);
            Value.Add(Value[Value.Count - 2]);
        }

        public void Clear()
        {
            Value.Clear();
        }

        public void Check(int expected)
        {
            if (Count < expected)
            {
                throw new StackException(expected, Count);
            }
        }

        public Sequence Get()
        {
            return Sequence.From(Value.ToList());
        }

        public void Dump()
        {
            G.C.WriteLine($"{this}");
        }

        public override string ToString()
        {
            return $"<{string.Join(" ", Get().Select(item => item.ToString() + ":" + item.GetType().Name))}>";
            //return $"<{string.Join(" ", Get())}>";
        }
    }
}
