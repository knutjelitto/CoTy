using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global
namespace CoTy.Objects.Implementations
{
    public partial class Implementation
    {
        public Sequence Take(object sequence, object count)
        {
            return Sequence.From(Loop(Enumerate(sequence), count));

            IEnumerable<object> Loop(IEnumerable<object> _sequence, object _count)
            {
                var zero = Dyn.Zero(_count);

                foreach (var value in _sequence)
                {
                    if (Dyn.Compare(zero, _count) < 0)
                    {
                        yield return value;

                        _count = Dyn.Pred(_count);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public Sequence Skip(object sequence, object count)
        {
            return Sequence.From(Loop(Enumerate(sequence), count));

            IEnumerable<object> Loop(IEnumerable<object> _sequence, object _count)
            {
                foreach (var value in _sequence)
                {
                    if (Dyn.Compare(_count, Integer.Zero) > 0)
                    {
                        _count = Dyn.Pred(_count);
                    }
                    else
                    {
                        yield return value;

                    }
                }
            }
        }

        public Sequence Range(object from, object count)
        {
            return Sequence.From(Loop(from, count));

            IEnumerable<object> Loop(object _from, object _count)
            {
                var zero = Dyn.Zero(_count);
                while (Dyn.Compare(_count, zero) > 0)
                {
                    yield return _from;

                    _count = Dyn.Pred(_count);
                    _from = Dyn.Succ(_from);
                }
            }
        }

        public Sequence Upto(object from, object upto)
        {
            return Sequence.From(Loop(from, upto));

            IEnumerable<object> Loop(object _from, object _upto)
            {
                while (Dyn.Compare(_from, _upto) <= 0)
                {
                    yield return _from;

                    _from = Dyn.Succ(_from);
                }
            }
        }

        public Sequence Up(object from)
        {
            return Sequence.From(Loop(from));

            IEnumerable<object> Loop(object _from)
            {
                while (true)
                {
                    yield return _from;

                    _from = Dyn.Succ(_from);
                }
                // ReSharper disable once IteratorNeverReturns
            }
        }

        public Sequence Repeat(object value, object count)
        {
            return Sequence.From(Loop(value, count));

            IEnumerable<object> Loop(object _value, object _count)
            {
                while (Dyn.Compare(_count, Integer.Zero) > 0)
                {
                    yield return _value;

                    _count = Dyn.Pred(_count);
                }
            }
        }

        public void Reduce(Context context, Stack stack, object sequence, object action)
        {
            var first = true;
            foreach (var value in Enumerate(sequence))
            {
                stack.Push(value);
                if (!first)
                {
                    Cobject.Apply(context, stack, action);
                }
                else
                {
                    first = false;
                }
            }
        }

        public void Map(Context context, Stack stack, object sequence, object action)
        {
            object Eval(object value)
            {
                stack.Push(value);
                Cobject.Apply(context, stack, action);
                return stack.Pop();
            }

            stack.Push(Sequence.From(Enumerate(sequence).Select(Eval)));
        }

        public void Each(Context context, Stack stack, object sequence, object action)
        {
            foreach (var value in Enumerate(sequence))
            {
                stack.Push(value);
                Cobject.Apply(context, stack, action);
            }
        }

        public Sequence Concat(object single1, object single2)
        {
            return Sequence.From(Enumerate(single1).Concat(Enumerate(single2)));
        }

        public void Unquote(Stack stack, object sequence)
        {
        }
    }
}
