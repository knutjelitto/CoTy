using System.Collections.Generic;
using System.Linq;

using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class SequenceModule : Module
    {
        public SequenceModule() : base("sequence") { }

        public override Context Reflect(Context into)
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
                    foreach (var value in Enumerate(values))
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
                    foreach (var value in Enumerate(values))
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

            Define(into, "forever", (object value) =>
            {
                return Sequence.From(Loop(value));

                IEnumerable<object> Loop(object _value)
                {
                    while (true)
                    {
                        yield return _value;
                    }
                }
            });
            
            Define(into, "repeat", (object value, dynamic count) =>
            {
                return Sequence.From(Loop(value, (Integer)count));

                IEnumerable<object> Loop(object _value, Integer _count)
                {
                    while (Integer.Zero.CompareTo(_count) < 0)
                    {
                        yield return value;
                        --_count;
                    }
                }
            });

            Define(into, "collapse", (Context context, Stack stack, object values, object action) =>
            {
                var first = true;
                foreach (var value in Enumerate(values))
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
            });

            Define(into, "reduce", (Context context, Stack stack, object values, object seed, object action) =>
            {
                stack.Push(seed);
                foreach (var value in Enumerate(values))
                {
                    stack.Push(value);
                    Apply(context, stack, action);
                }
            });

            return base.Reflect(into);
        }


        [Builtin("map", InArity = 2, OutArity = 1)]
        private static void Map(Context context, Stack stack)
        {
            var action = stack.Pop();
            var sequence = stack.Pop();

            var result = Sequence.From(Enumerate(sequence).Select(value => Eval(value, action)));

            stack.Push(result);

            object Eval(object _value, object _action)
            {
                stack.Push(_value);
                Apply(context, stack, _action);
                return stack.Pop();
            }
        }

        [Builtin("foreach")]
        private static void Each(Context context, Stack stack)
        {
            var action = stack.Pop();
            var sequence = stack.Pop();

            foreach (var value in Enumerate(sequence))
            {
                stack.Push(value);
                Apply(context, stack, action);
            }
        }

        [Builtin("count")]
        private static void Count(Context context, Stack stack)
        {
            var value = stack.Pop();

            var result = Enumerate(value).Count();

            stack.Push(result);
        }
    }
}
