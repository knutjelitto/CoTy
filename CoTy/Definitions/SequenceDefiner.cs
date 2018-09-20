using System.Collections.Generic;
using System.Linq;

using CoTy.Objects;

namespace CoTy.Definitions
{
    public class SequenceDefiner : Definer
    {
        public SequenceDefiner() : base("sequence")
        {
        }

        public override void Define(IScope into)
        {
            Define(into, ",", (scope, stack, value1, value2) => Sequence.From(value1.Enumerate().Concat(value2.Enumerate())));
            Define(into, ",,", (scope, stack, value1, value2, value3) => Sequence.From(value1.Enumerate().Concat(value2.Enumerate()).Concat(value3.Enumerate())));

            Define(into,
                   "up",
                   (dynamic value) =>
                   {
                       return Sequence.From(Loop((Integer) value));

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

            Define(into,
                   "upto",
                   (dynamic start, dynamic limit) =>
                   {
                       return Sequence.From(Loop((Integer) start, (Integer) limit));

                       IEnumerable<object> Loop(Integer current, Integer _limit)
                       {
                           while (current.CompareTo(_limit) <= 0)
                           {
                               yield return current;

                               current = ++current;
                           }
                       }
                   });


            Define(into,
                   "range",
                   (dynamic start, dynamic count) =>
                   {
                       return Sequence.From(Loop(start, (Integer) count));

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

            Define(into,
                   "take",
                   (object values, dynamic count) =>
                   {
                       return Sequence.From(Loop(values, (Integer) count));

                       IEnumerable<object> Loop(object _values, Integer _count)
                       {
                           foreach (var value in _values.Enumerate())
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

            Define(into,
                   "skip",
                   (object values, dynamic count) =>
                   {
                       return Sequence.From(Loop(values, (Integer) count));

                       IEnumerable<object> Loop(object _values, Integer _count)
                       {
                           foreach (var value in _values.Enumerate())
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

            Define(into,
                   "forever",
                   value =>
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

            Define(into,
                   "repeat",
                   (object value, dynamic count) =>
                   {
                       return Sequence.From(Loop(value, (Integer) count));

                       IEnumerable<object> Loop(object _value, Integer _count)
                       {
                           while (Integer.Zero.CompareTo(_count) < 0)
                           {
                               yield return _value;

                               --_count;
                           }
                       }
                   });

            Define(into,
                   "collapse",
                   (scope, stack, values, action) =>
                   {
                       var first = true;
                       foreach (var value in values.Enumerate())
                       {
                           stack.Push(value);
                           if (!first)
                           {
                               action.Apply(scope, stack);
                           }
                           else
                           {
                               first = false;
                           }
                       }
                   });

            Define(into,
                   "reduce",
                   (scope, stack, values, seed, action) =>
                   {
                       stack.Push(seed);
                       foreach (var value in values.Enumerate())
                       {
                           stack.Push(value);
                           action.Apply(scope, stack);
                       }
                   });

            Define(into,
                   "select",
                   (scope, stack, values, action) =>
                   {
                       var result =
                           (values is IEnumerable<object> sequence)
                               ? Sequence.From(sequence.Select(value => Eval(value, action)))
                               : Eval(values, action);

                       stack.Push(result);

                       object Eval(object _value, object _action)
                       {
                           stack.Push(_value);
                           _action.Apply(scope, stack);
                           return stack.Pop();
                       }
                   });

            Define(into,
                   "foreach",
                   (scope, stack, sequence, action) =>
                   {
                       foreach (var value in sequence.Enumerate())
                       {
                           stack.Push(value);
                           action.Apply(scope, stack);
                       }
                   });

            Define(into,
                   "count",
                   (scope, stack, values) =>
                   {
                       var count = Integer.Zero;

                       foreach (var _ in values.Enumerate())
                       {
                           ++count;
                       }

                       stack.Push(count);
                   });

            Define(into,
                   "fr",
                   (scope, stack, values) =>
                   {
                       var count = Integer.Zero;

                       foreach (var _ in values.Enumerate())
                       {
                           ++count;
                       }

                       stack.Push(count);
                   });
        }
    }
}
