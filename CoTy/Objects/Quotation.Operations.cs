using System.Linq;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Quotation
    {

        public Quotation Add(Cobject other)
        {
            return new Quotation(null, this.Concat(other));
        }

        public Quotation CoAdd(Cobject other)
        {
            return new Quotation(null, other.Concat(this));
        }
    }
}
