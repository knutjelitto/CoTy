using System.Collections.Generic;
using System.Linq;

namespace CoTy.Objects.Impl
{
    public partial class CobjectImpl
    {
        public CobjectImpl()
        {
        }

        public dynamic Dyn => this;

        protected static IEnumerable<T> Enumerate<T>(T value)
        {
            return value as IEnumerable<T> ?? Enumerable.Repeat(value, 1);
        }
    }
}
