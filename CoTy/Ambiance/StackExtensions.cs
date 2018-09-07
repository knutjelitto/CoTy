using CoTy.Objects;

namespace CoTy.Ambiance
{
    public static class StackExtensions
    {
        public static (Cobject x, Cobject y) Pop2(this IStack This)
        {
            var y = This.Pop();
            var x = This.Pop();

            return (x, y);
        }

        public static dynamic Popd(this IStack This)
        {
            return This.Pop();
        }

        public static (dynamic x, dynamic y) Pop2d(this IStack This)
        {
            var y = This.Pop();
            var x = This.Pop();

            return (x, y);
        }

        public static T Pop<T>(this IStack This) where T : Cobject
        {
            return (T)This.Pop();
        }
    }
}
