using CoTy.Objects;

namespace CoTy.Implementations
{
    public class IntegerImpl : Implementation
    {
        public int? Compare(Integer value1, Integer value2)
        {
            return value1.Value.CompareTo(value2.Value);
        }
    }
}
