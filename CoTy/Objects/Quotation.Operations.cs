using System.Linq;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class Quotation
    {
        public Quotation Concatenate(Cobject other)
        {
            return new Quotation(Lexical, this.Concat(other));
        }

        public Quotation CoConcatenate(Cobject other)
        {
            return new Quotation(Lexical, other.Concat(this));
        }
    }
}
