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
    }
}
