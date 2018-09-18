using CoTy.Objects;

namespace CoTy.Definitions
{
    public class BindingDefiner : Definer
    {
        public BindingDefiner() : base("binding"){}

        public override void Define(IContext into)
        {
            Define(into,
                   "def",
                   (context, stack, value, symbol) => { context.Define(GetSymbol(symbol), value); });
            Define(into,
                   "undef",
                   (context, stack, symbol) => { context.Undefine(GetSymbol(symbol)); });
            Define(into,
                   "set",
                   (context, stack, value, symbol) => { context.Update(GetSymbol(symbol), value); });
            Define(into,
                   "def?",
                   (context, stack, symbol) => TryGetSymbol(symbol, out var asSymbol) && context.IsDefined(asSymbol));
            Define(into,
                   "value",
                   (context, stack, symbol) => context.Find(GetSymbol(symbol)).Value);
            Define(into,
                   "seal",
                   (context, stack, symbol) => { context.Find(GetSymbol(symbol)).IsSealed = true; });
            Define(into,
                   "unseal",
                   (context, stack, symbol) => { context.Find(GetSymbol(symbol)).IsSealed = false; });
            Define(into,
                   "sealed?",
                   (context, stack, symbol) => context.Find(GetSymbol(symbol)).IsSealed);
        }
    }
}
