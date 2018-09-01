using CoTy.Ambiance;

namespace CoTy.Objects
{
    public class CoSelfish<TClr> : CoObject<TClr>
    {
        public CoSelfish(TClr value) : base(value)
        {
        }

        public override void Apply(AmScope scope, AmStack stack)
        {
            stack.Push(this);
        }
    }
}
