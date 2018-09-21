using CoTy.Objects;

namespace CoTy.Definitions
{
    public class CoreBinder : Core
    {
        public CoreBinder() : base("binding"){}

        public override void Define(Maker into)
        {
            into.Define(
                   "def",
                   (scope, stack, value, symbol) => { scope.Define(symbol.GetSymbol(), value); });
            into.Define(
                   "undef",
                   (scope, stack, symbol) => { scope.Undefine(symbol.GetSymbol()); });
            into.Define(
                   "set",
                   (scope, stack, value, symbol) => { scope.Update(symbol.GetSymbol(), value); });
            into.Define(
                   "def?",
                   (scope, stack, symbol) => symbol.TryGetSymbol(out var asSymbol) && scope.IsDefined(asSymbol));
            into.Define(
                   "value",
                   (scope, stack, symbol) => scope.Find(symbol.GetSymbol()).Value);
            into.Define(
                   "seal",
                   (scope, stack, symbol) => { scope.Find(symbol.GetSymbol()).IsSealed = true; });
            into.Define(
                   "unseal",
                   (scope, stack, symbol) => { scope.Find(symbol.GetSymbol()).IsSealed = false; });
            into.Define(
                   "sealed?",
                   (scope, stack, symbol) => scope.Find(symbol.GetSymbol()).IsSealed);
            into.Define(
                   "opaque",
                   (scope, stack, symbol) => { scope.Find(symbol.GetSymbol()).IsOpaque = true; });
            into.Define(
                   "unopaque",
                   (scope, stack, symbol) => { scope.Find(symbol.GetSymbol()).IsOpaque = false; });
            into.Define(
                   "opaque?",
                   (scope, stack, symbol) => scope.Find(symbol.GetSymbol()).IsOpaque);
        }
    }
}
