using CoTy.Errors;

namespace CoTy.Objects
{
    public static class StackExtensions
    {
        public static object PopApply(this IStack This, IContext context)
        {
            var value = This.Pop();
            value.Apply(context, This);
            value = This.Pop();

            return value;
        }
    }
}
