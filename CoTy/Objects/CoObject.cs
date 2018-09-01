using CoTy.Ambiance;

namespace CoTy.Objects
{
    public abstract class CoObject
    {
        public abstract void Apply(AmScope scope, AmStack stack);
    }

    public abstract class CoObject<TClr> : CoObject
    {
        protected CoObject(TClr value)
        {
            Value = value;
        }

        public TClr Value { get; }
    }
}
