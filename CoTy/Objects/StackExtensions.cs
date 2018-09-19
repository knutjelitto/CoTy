using CoTy.Errors;

namespace CoTy.Objects
{
    public static class StackExtensions
    {
        public static object PopApply(this IStack This, IScope scope)
        {
            var value = This.Pop();
            value.Apply(scope, This);
            value = This.Pop();

            return value;
        }
    }
}
