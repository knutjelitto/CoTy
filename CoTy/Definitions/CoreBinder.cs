using CoTy.Objects;

namespace CoTy.Definitions
{
    public class CoreBinder : Core
    {
        public CoreBinder() : base("binding"){}

        public override void Define(Maker into)
        {
            into.Define("def", (scope, stack, value, symbol) => { scope.Define(symbol.GetSymbol(), value); });
            into.Define("undef", (scope, stack, symbol) => { scope.Undefine(symbol.GetSymbol()); });
            into.Define("set", (scope, stack, value, symbol) => { scope.Update(symbol.GetSymbol(), value); });
            into.Define("def?", (scope, stack, symbol) => Bool.From(symbol.TryGetSymbol(out var asSymbol) && scope.IsDefined(asSymbol)));
            into.Define("value", (scope, stack, symbol) => scope.Find(symbol.GetSymbol()).Value);
            into.Define("seal", (scope, stack, symbol) => { scope.Find(symbol.GetSymbol()).IsSealed = Bool.True; });
            into.Define("unseal", (scope, stack, symbol) => { scope.Find(symbol.GetSymbol()).IsSealed = Bool.False; });
            into.Define("sealed?", (scope, stack, symbol) => scope.Find(symbol.GetSymbol()).IsSealed);
            into.Define("opaque", (scope, stack, symbol) => { scope.Find(symbol.GetSymbol()).IsOpaque = Bool.True; });
            into.Define("unopaque", (scope, stack, symbol) => { scope.Find(symbol.GetSymbol()).IsOpaque = Bool.False; });
            into.Define("opaque?", (scope, stack, symbol) => scope.Find(symbol.GetSymbol()).IsOpaque);
        }
    }
}
