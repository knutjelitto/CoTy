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
                count = count.Succ(count);
            }

            return count;
        }

        public Sequence Take(Cobject seq, Cobject cnt)
        {
            return new Sequence(Loop(seq, cnt));

            IEnumerable<Cobject> Loop(Cobject _seq, Cobject _cnt)
            {
                foreach (var value in _seq)
                {
                    if (Eval.Compare(_cnt, Integer.Zero) > 0)
                    {
                        yield return value;

                        _cnt = Eval.Pred(_cnt);
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

            IEnumerable<Cobject> Loop(Cobject _seq, Cobject _cnt)
            {
                foreach (var value in _seq)
                {
                    if (Eval.Compare(_cnt, Integer.Zero) > 0)
                    {
                        _cnt = Eval.Pred(_cnt);
                    }
                    else
                    {
                        yield return value;

                    }
                }
            }
        }

        public Sequence Upto(Cobject from, Cobject upto)
        {
            return new Sequence(Loop(from, upto));

            IEnumerable<Cobject> Loop(Cobject _from, Cobject _upto)
            {
                while (Eval.Compare(_from, _upto) <= 0)
                {
                    yield return _from;

                    _from = Eval.Succ(_from);
                }
            }
        }

        public Sequence Up(Cobject from)
        {
            return new Sequence(Loop(from));

            IEnumerable<Cobject> Loop(Cobject _from)
            {
                while (true)
                {
                    yield return _from;

                    _from = Eval.Succ(_from);
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

            IEnumerable<Cobject> Loop(Cobject _value, Cobject _count)
            {
                while (Eval.Compare(_count, Integer.Zero) > 0)
                {
                    yield return _value;

                    _count = Eval.Pred(_count);
                }
            }
        }
    }
}
