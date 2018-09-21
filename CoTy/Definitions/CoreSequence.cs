using System.Collections.Generic;
using System.Linq;

using CoTy.Objects;

namespace CoTy.Definitions
{
    public class CoreSequence : Core
    {
        public CoreSequence() : base("sequence")
        {
        }

        public override void Define(Maker into)
        {
            into.Define("any?", value => !(value is IEnumerable<object> enumerable) || enumerable.Any());
            into.Define("empty?", value => value is IEnumerable<object> enumerable && !enumerable.Any());
            into.Define("atomic?", value => !(value is IEnumerable<object>));

            into.Define(
                   "single?",
                   value =>
                   {
                       if (value is IEnumerable<object> enumerable)
                       {
                           using (var enumerator = enumerable.GetEnumerator())
                           {
                               return enumerator.MoveNext() && !enumerator.MoveNext();
                           }
                       }
                       return true;
                   });

            into.Define(
                   "first-rest",
                   (scope, stack, value) =>
                   {
                       if (value is IEnumerable<object> enumerable)
                       {
                           // ReSharper disable once PossibleMultipleEnumeration
                           var first = enumerable.FirstOrDefault() ?? Sequence.Empty;
                           // ReSharper disable once PossibleMultipleEnumeration
                           var rest = Sequence.From(enumerable.Skip(1));
                           stack.Push(first);
                           stack.Push(rest);
                       }
                       else
                       {
                           stack.Push(value);
                           stack.Push(Sequence.Empty);
                       }
                   });

            into.Define(",", (scope, stack, value1, value2) => Sequence.From(value1.Enumerate().Concat(value2.Enumerate())));
            into.Define(",,", (scope, stack, value1, value2, value3) => Sequence.From(value1.Enumerate().Concat(value2.Enumerate()).Concat(value3.Enumerate())));

            into.Define(
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

            into.Define(
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

            into.Define(
                   "range",
                   (dynamic start, dynamic count) =>
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

            into.Define(
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

            into.Define(
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

            into.Define(
                   "collapse",
                   (scope, stack, values, action) =>
                   {
                       var first = true;
                       foreach (var value in values.Enumerate())
                       {
                           stack.Push(value);
                           if (!first)
                           {
                               action.Eval(scope, stack);
                           }
                           else
                           {
                               first = false;
                           }
                       }

                       if (first)
                       {
                           stack.Push(Sequence.From());
                       }
                   });

            into.Define(
                   "reduce",
                   (scope, stack, values, seed, action) =>
                   {
                       stack.Push(seed);
                       foreach (var value in values.Enumerate())
                       {
                           stack.Push(value);
                           action.Eval(scope, stack);
                       }
                   });

            into.Define(
                   "select",
                   (scope, stack, values, action) =>
                   {
                       var result =
                           values is IEnumerable<object> sequence
                               ? Sequence.From(sequence.Select(value => Eval(value, action)))
                               : Eval(values, action);

                       stack.Push(result);

                       object Eval(object _value, object _action)
                       {
                           stack.Push(_value);
                           _action.Eval(scope, stack);
                           return stack.Pop();
                       }
                   });


            into.Define(
                   "where",
                   (scope, stack, values, predicate) =>
                   {
                       var sequence = values is IEnumerable<object> enumerable
                                          ? enumerable
                                          : Enumerable.Repeat(values, 1);

                       var result = Sequence.From(sequence.Where(value => Eval(value, predicate)));

                       stack.Push(result);

                       bool Eval(object _value, object _predicate)
                       {
                           stack.Push(_value);
                           _predicate.Eval(scope, stack);
                           return stack.Pop() is bool b && b;
                       }
                   });

            into.Define(
                   "foreach",
                   (scope, stack, sequence, action) =>
                   {
                       foreach (var value in sequence.Enumerate())
                       {
                           stack.Push(value);
                           action.Eval(scope, stack);
                       }
                   });

            into.Define(
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
        }
    }
}
