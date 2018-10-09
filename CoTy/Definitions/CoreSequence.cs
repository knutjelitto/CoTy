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
            into.Define("any?", value => Bool.From(!(value is IEnumerable<Cobject> enumerable) || enumerable.Any()));
            into.Define("empty?", value => Bool.From(value is IEnumerable<Cobject> enumerable && !enumerable.Any()));
            into.Define("atomic?", value => Bool.From(!(value is IEnumerable<Cobject>)));

            into.Define(
                   "single?",
                   value =>
                   {
                       if (value is IEnumerable<Cobject> enumerable)
                       {
                           using (var enumerator = enumerable.GetEnumerator())
                           {
                               return Bool.From(enumerator.MoveNext() && !enumerator.MoveNext());
                           }
                       }
                       return Bool.True;
                   });

            into.Define(
                   "first-rest",
                   (scope, stack, value) =>
                   {
                       if (value is IEnumerable<Cobject> enumerable)
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
                   (Cobject values, Cobject count) =>
                   {
                       return Sequence.From(Loop(values, (Integer)(dynamic)count));

                       IEnumerable<Cobject> Loop(Cobject _values, Integer _count)
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
                   (Cobject values, Cobject count) =>
                   {
                       return Sequence.From(Loop(values, (Integer) (dynamic)count));

                       IEnumerable<Cobject> Loop(Cobject _values, Integer _count)
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
                   (Cobject start, Cobject count) =>
                   {
                       return Sequence.From(Loop(start, (Integer)(dynamic)count));

                       IEnumerable<Cobject> Loop(dynamic current, Integer _count)
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

                       IEnumerable<Cobject> Loop(Cobject _value)
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
                   (Cobject value, Cobject count) =>
                   {
                       return Sequence.From(Loop(value, (Integer)(dynamic)count));

                       IEnumerable<Cobject> Loop(Cobject _value, Integer _count)
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
                           stack.Push(Sequence.Empty);
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
                           values is IEnumerable<Cobject> sequence
                               ? Sequence.From(sequence.Select(value => Eval(value, action)))
                               : Eval(values, action);

                       stack.Push(result);

                       Cobject Eval(Cobject _value, Cobject _action)
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
                       var sequence = values is IEnumerable<Cobject> enumerable
                                          ? enumerable
                                          : Enumerable.Repeat(values, 1);

                       var result = Sequence.From(sequence.Where(value => Eval(value, predicate)));

                       stack.Push(result);

                       bool Eval(Cobject _value, Cobject _predicate)
                       {
                           stack.Push(_value);
                           _predicate.Eval(scope, stack);
                           return stack.Pop() is Bool b && b.Value;
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
