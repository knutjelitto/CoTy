using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public abstract partial class Cobject
    {
        public Integer Count(Cobject sequence)
        {
            var count = Integer.Zero;

            foreach (var _ in sequence)
            {
                count = count.Succ();
            }

            return count;
        }

        public Sequence Take(Cobject seq, Cobject cnt)
        {
            return new Sequence(Loop(seq, cnt));

            IEnumerable<Cobject> Loop(Cobject _seq, dynamic _cnt)
            {
                foreach (var value in _seq)
                {
                    if (_cnt.Greater(Integer.Zero))
                    {
                        yield return value;

                        _cnt = _cnt.Pred();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public Sequence Skip(Cobject seq, Cobject cnt)
        {
            return new Sequence(Loop(seq, cnt));

            IEnumerable<Cobject> Loop(Cobject _seq, dynamic _cnt)
            {
                foreach (var value in _seq)
                {
                    if (_cnt.Greater(Integer.Zero))
                    {
                        _cnt = _cnt.Pred();
                    }
                    else
                    {
                        yield return value;

                    }
                }
            }
        }

        public Sequence Upto(dynamic from, dynamic upto)
        {
            return new Sequence(Loop(from, upto));

            IEnumerable<Cobject> Loop(dynamic _from, dynamic _upto)
            {
                while (_from.LessOrEquals(_upto) is Bool condition && condition)
                {
                    yield return _from;

                    _from = _from.Succ();
                }
            }
        }

        public Sequence Up(dynamic from)
        {
            return new Sequence(Loop(from));

            IEnumerable<Cobject> Loop(dynamic _from)
            {
                while (true)
                {
                    yield return _from;

                    _from = _from.Succ();
                }
            }
        }

        public Sequence Concat(Cobject seq1, Cobject seq2)
        {
            return new Sequence(seq1.Concat(seq2));
        }

        public Sequence Repeat(Cobject value, Cobject count)
        {
            return new Sequence(Loop(value, count));

            IEnumerable<Cobject> Loop(Cobject _value, dynamic _count)
            {
                while (_count.Greater(Integer.Zero))
                {
                    yield return _value;

                    _count = _count.Pred();
                }
            }
        }
    }
}
