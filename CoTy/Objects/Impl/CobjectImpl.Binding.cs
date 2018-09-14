using System;

// ReSharper disable UnusedMember.Global
namespace CoTy.Objects.Impl
{
    public partial class CobjectImpl
    {
        public void Define(Context context, Stack stack, Symbol symbol, object value)
        {
            context.Define(symbol, value);
        }

        public void Undefine(Context context, Stack stack, Symbol symbol, object value)
        {
            context.Undefine(symbol);
        }
    }
}
