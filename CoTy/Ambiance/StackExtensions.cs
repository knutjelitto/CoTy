using CoTy.Errors;
using CoTy.Objects;

namespace CoTy.Ambiance
{
    public static class StackExtensions
    {
        public static (Cobject x, Cobject y) Pop2(this AmStack This)
        {
            if (This.Count < 2)
            {
                throw new StackException(2, This.Count);
            }

            var y = This.Pop();
            var x = This.Pop();

            return (x, y);
        }

        public static dynamic Popd(this AmStack This)
        {
            return This.Pop();
        }

        public static (dynamic x, dynamic y) Pop2d(this AmStack This)
        {
            if (This.Count < 2)
            {
                throw new StackException(2, This.Count);
            }

            var y = This.Pop();
            var x = This.Pop();

            return (x, y);
        }

        public static T Pop<T>(this AmStack This) where T : Cobject
        {
            return (T)This.Pop();
        }
    }
}
