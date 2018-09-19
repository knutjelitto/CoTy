namespace CoTy.Objects
{
    public static class ObjectExtensions
    {
        public static void Apply(this object This, IScope scope, IStack stack)
        {
            if (This is Cobject cvalue)
            {
                cvalue.Apply(scope, stack);
            }
            else
            {
                stack.Push(This);
            }
        }

        public static void Lambda(this object This, IScope scope, IStack stack)
        {
            if (This is Cobject cvalue)
            {
                cvalue.Lambda(scope, stack);
            }
            else
            {
                stack.Push(This);
            }
        }
    }
}
