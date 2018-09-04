using System.Linq;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Quotation
    {

        public Quotation Add(Cobject other)
        {
            return new Quotation(this.Concat(other));
        }

        public Quotation CoAdd(Cobject other)
        {
            return new Quotation(other.Concat(this));
        }
    }
}
