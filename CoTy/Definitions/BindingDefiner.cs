using CoTy.Objects;

namespace CoTy.Definitions
{
    public class BindingDefiner : Definer
    {
        public BindingDefiner() : base("binding"){}

        public override void Define(IScope into)
        {
            Define(into,
                   "def",
                   (scope, stack, value, symbol) => { scope.Define(GetSymbol(symbol), value); });
            Define(into,
                   "undef",
                   (scope, stack, symbol) => { scope.Undefine(GetSymbol(symbol)); });
            Define(into,
                   "set",
                   (scope, stack, value, symbol) => { scope.Update(GetSymbol(symbol), value); });
            Define(into,
                   "def?",
                   (scope, stack, symbol) => TryGetSymbol(symbol, out var asSymbol) && scope.IsDefined(asSymbol));
            Define(into,
                   "value",
                   (scope, stack, symbol) => scope.Find(GetSymbol(symbol)).Value);
            Define(into,
                   "seal",
                   (scope, stack, symbol) => { scope.Find(GetSymbol(symbol)).IsSealed = true; });
            Define(into,
                   "unseal",
                   (scope, stack, symbol) => { scope.Find(GetSymbol(symbol)).IsSealed = false; });
            Define(into,
                   "sealed?",
                   (scope, stack, symbol) => scope.Find(GetSymbol(symbol)).IsSealed);
        }
    }
}
