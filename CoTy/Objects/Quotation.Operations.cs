using System.Linq;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Quotation
    {
        public Quotation Concatenate(Cobject other)
        {
            return new Quotation(null, this.Concat(other));
        }

        public Quotation CoConcatenate(Cobject other)
        {
            return new Quotation(null, other.Concat(this));
        }
    }
}
