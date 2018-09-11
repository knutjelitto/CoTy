using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Sequence
    {
        public Sequence Concatenate(Cobject other)
        {
            return new Sequence(this.Concat(other));
        }

        public Sequence CoConcatenate(Cobject other)
        {
            return new Sequence(other.Concat(this));
        }

        public static Sequence Upto(dynamic from, dynamic upto)
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

        public static Sequence Up(dynamic from)
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
    }
}
