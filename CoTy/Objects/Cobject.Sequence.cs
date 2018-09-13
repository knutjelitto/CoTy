using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Cobject
    {
        public Integer Count(Cobject sequence)
        {
            var count = Integer.Zero;

            foreach (var _ in Enumerate(sequence))
            {
                count = count.Succ(count);
            }

            return count;
        }

        public Sequence Take(Cobject sequence, Cobject count)
        {
            return Sequence.From(Loop(Enumerate(sequence), count));

            IEnumerable<Cobject> Loop(IEnumerable<Cobject> _sequence, Cobject _count)
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

        public Sequence Skip(Cobject sequence, Cobject count)
        {
            return Sequence.From(Loop(Enumerate(sequence), count));

            IEnumerable<Cobject> Loop(IEnumerable<Cobject> _sequence, Cobject _count)
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

        public Sequence Range(Cobject from, Cobject count)
        {
            return Sequence.From(Loop(from, count));

            IEnumerable<Cobject> Loop(Cobject _from, Cobject _count)
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

        public Sequence Upto(Cobject from, Cobject upto)
        {
            return Sequence.From(Loop(from, upto));

            IEnumerable<Cobject> Loop(Cobject _from, Cobject _upto)
            {
                while (Dyn.Compare(_from, _upto) <= 0)
                {
                    yield return _from;

                    _from = Dyn.Succ(_from);
                }
            }
        }

        public Sequence Up(Cobject from)
        {
            return Sequence.From(Loop(from));

            IEnumerable<Cobject> Loop(Cobject _from)
            {
                while (true)
                {
                    yield return _from;

                    _from = Dyn.Succ(_from);
                }
                // ReSharper disable once IteratorNeverReturns
            }
        }

        public Sequence Repeat(Cobject value, Cobject count)
        {
            return Sequence.From(Loop(value, count));

            IEnumerable<Cobject> Loop(Cobject _value, Cobject _count)
            {
                while (Dyn.Compare(_count, Integer.Zero) > 0)
                {
                    yield return _value;

                    _count = Dyn.Pred(_count);
                }
            }
        }

        public void Reduce(Context context, Stack stack, Cobject sequence, Cobject action)
        {
            var first = true;
            foreach (var value in Enumerate(sequence))
            {
                stack.Push(value);
                if (!first)
                {
                    Apply(context, stack, action);
                }
                else
                {
                    first = false;
                }
            }
        }

        public void Map(Context context, Stack stack, Cobject sequence, Cobject action)
        {
            Cobject Eval(Cobject value)
            {
                stack.Push(value);
                Apply(context, stack, action);
                return stack.Pop();
            }

            stack.Push(Sequence.From(Enumerate(sequence).Select(Eval)));
        }

        public void Each(Context context, Stack stack, Cobject sequence, Cobject action)
        {
            foreach (var value in Enumerate(sequence))
            {
                stack.Push(value);
                Apply(context, stack, action);
            }
        }

        public Sequence Concat(Cobject single1, Cobject single2)
        {
            return Sequence.From(Enumerate(single1).Concat(Enumerate(single2)));
        }

        public void Dequote(Stack stack, Sequence sequence)
        {
            foreach (var value in sequence)
            {
                stack.Push(value);
            }
        }

        public void Dequote(Stack stack, Cobject value)
        {
            stack.Push(value);
        }
    }
}
