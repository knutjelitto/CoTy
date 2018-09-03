using System.Linq;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects
{
    public partial class CoQuotation
    {

        public CoQuotation Add(CoTuple other)
        {
            return new CoQuotation(this.Concat(other));
        }

        public CoQuotation CoAdd(CoTuple other)
        {
            return new CoQuotation(other.Concat(this));
        }
    }
}
