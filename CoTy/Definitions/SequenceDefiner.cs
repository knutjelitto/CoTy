using System.Collections.Generic;
using System.Linq;

using CoTy.Objects;

namespace CoTy.Definitions
{
    public class SequenceDefiner : Definer
    {
        public SequenceDefiner() : base("sequence") { }

        public override Context Define(Context into)
        {
            Define(into, "up", (dynamic value) =>
            {
                return Sequence.From(Loop((Integer)value));

                IEnumerable<object> Loop(Integer current)
                {
                    while (true)
                    {
                        yield return current;
                        value = ++current;
                    }
                    // ReSharper disable once IteratorNeverReturns
                }
            });

            Define(into, "upto", (dynamic start, dynamic limit) =>
            {
                return Sequence.From(Loop((Integer)start, (Integer)limit));

                IEnumerable<object> Loop(Integer current, Integer _limit)
                {
                    while (current.CompareTo(_limit) <= 0)
                    {
                        yield return current;
                        current = ++current;
                    }
                }
            });


            Define(into, "range", (dynamic start, dynamic count) =>
            {
                return Sequence.From(Loop(start, (Integer)count));

                IEnumerable<object> Loop(dynamic current, Integer _count)
                {
                    while (Integer.Zero.CompareTo(_count) < 0)
                    {
                        yield return current;
                        current = ++current;
                        --_count;
                    }
                }
            });

            Define(into, "take", (object values, dynamic count) =>
            {
                return Sequence.From(Loop(values, (Integer)count));

                IEnumerable<object> Loop(object _values, Integer _count)
                {
                    foreach (var value in Enumerate(_values))
                    {
                        if (Integer.Zero.CompareTo(_count) < 0)
                        {
                            yield return value;
                            --_count;
                        }
                        if (Integer.Zero.CompareTo(_count) >= 0)
                        {
                            yield break;
                        }
                    }
                }
            });

            Define(into, "skip", (object values, dynamic count) =>
            {
                return Sequence.From(Loop(values, (Integer)count));

                IEnumerable<object> Loop(object _values, Integer _count)
                {
                    foreach (var value in Enumerate(_values))
                    {
                        if (Integer.Zero.CompareTo(_count) < 0)
                        {
                            --_count;
                        }
                        else
                        {
                            yield return value;
                        }
                    }
                }
            });

            Define(into, "forever", value =>
            {
                return Sequence.From(Loop(value));

                IEnumerable<object> Loop(object _value)
                {
                    while (true)
                    {
                        yield return _value;
                    }
                    // ReSharper disable once IteratorNeverReturns
                }
            });
            
            Define(into, "repeat", (object value, dynamic count) =>
            {
                return Sequence.From(Loop(value, (Integer)count));

                IEnumerable<object> Loop(object _value, Integer _count)
                {
                    while (Integer.Zero.CompareTo(_count) < 0)
                    {
                        yield return _value;
                        --_count;
                    }
                }
            });

            Define(into, "collapse", (context, stack, values, action) =>
            {
                var first = true;
                foreach (var value in Enumerate(values))
                {
                    stack.Push(value);
                    if (!first)
                    {
                        action.Apply(context, stack);
                    }
                    else
                    {
                        first = false;
                    }
                }
            });

            Define(into, "reduce", (context, stack, values, seed, action) =>
            {
                stack.Push(seed);
                foreach (var value in Enumerate(values))
                {
                    stack.Push(value);
                    action.Apply(context, stack);
                }
            });

            Define(into,
                   "map",
                   (context, stack, sequence, action) =>
                   {
                       var result = Sequence.From(Enumerate(sequence).Select(value => Eval(value, action)));

                       stack.Push(result);

                       object Eval(object _value, object _action)
                       {
                           stack.Push(_value);
                           _action.Apply(context, stack);
                           return stack.Pop();
                       }
                   });

            Define(into,
                   "foreach",
                   (context, stack, sequence, action) =>
                   {
                       foreach (var value in Enumerate(sequence))
                       {
                           stack.Push(value);
                           action.Apply(context, stack);
                       }
                   });


            Define(into,
                   "count",
                   (context, stack, values) =>
                   {
                       var count = Integer.Zero;

                       foreach (var _ in Enumerate(values))
                       {
                           ++count;
                       }
                       stack.Push(count);
                   });

            return base.Define(into);
        }
    }
}
