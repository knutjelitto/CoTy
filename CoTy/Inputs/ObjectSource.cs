using CoTy.Objects;

namespace CoTy.Inputs
{
    public class ObjectSource : ItemSource<Cobject>
    {
        public ObjectSource(ItemStream<Cobject> objectStream)
            : base(objectStream)
        {
        }
    }
}
