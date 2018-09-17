using CoTy.Objects;

namespace CoTy.Definitions
{
    public class BindingDefiner : Definer
    {
        public BindingDefiner() : base("binding"){}

        public override Context Define(Context @into)
        {
            Define(into,
                   "def",
                   (context, stack, symbol, value) => { context.Define(GetSymbol(symbol), value); });
            Define(into,
                   "undef",
                   (context, stack, symbol) => { context.Undefine(GetSymbol(symbol)); });
            Define(into,
                   "set",
                   (context, stack, symbol, value) => { context.Update(GetSymbol(symbol), value); });
            Define(into,
                   "def?",
                   (context, stack, symbol) => TryGetSymbol(symbol, out var asSymbol) && context.IsDefined(asSymbol));
            Define(into,
                   "value",
                   (context, stack, symbol) => context.Find(GetSymbol(symbol)).Value);

            return base.Define(@into);
        }
    }
}
