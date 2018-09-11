using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class BindingModule : Module
    {
        public BindingModule(Context parent) : base(parent, "binding")
        {
        }

        [Builtin("def")]
        private static void Define(Context context, Stack stack)
        {
            var symbol = GetSymbol(stack.Pop());
            var value = stack.Pop();

            context.Define(symbol, value);
        }

        [Builtin("undef")]
        private static void Undefine(Context context, Stack stack)
        {
            var symbol = GetSymbol(stack.Pop());

            context.Undefine(symbol);
        }

        [Builtin("set")]
        private static void Set(Context context, Stack stack)
        {
            var symbol = GetSymbol(stack.Pop());
            var value = stack.Pop();


            context.Update(symbol, value);
        }

        [Builtin("def?")]
        private static void DefinedPred(Context context, Stack stack)
        {
            if (TryGetSymbol(stack.Pop(), out var symbol))
            {
                stack.Push((Bool) context.IsDefined(symbol));
            }
            stack.Push(Bool.False);
        }

        [Builtin("value")]
        private static void GetValue(Context context, Stack stack)
        {
            var symbol = GetSymbol(stack.Pop());

            var found = context.Find(symbol);

            stack.Push(found.Value);
        }
    }
}
