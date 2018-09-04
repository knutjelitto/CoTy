using System.Linq;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Quotation
    {

        public Quotation Add(CoTuple other)
        {
            return new Quotation(this.Concat(other));
        }

        public Quotation CoAdd(CoTuple other)
        {
            return new Quotation(other.Concat(this));
        }
    }
}
