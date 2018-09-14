// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global
namespace CoTy.Objects.Implementations
{
    public partial class Implementation
    {
        public void Quote(Context context, Stack stack)
        {
            var value = stack.Pop();

            var result = Closure.From(context, value);

            stack.Push(result);
        }

        public void Unquote(Context context, Stack stack)
        {
            var sequence = stack.Pop();

            foreach (var value in Enumerate(sequence))
            {
                stack.Push(value);
            }
        }

        public void Apply(Context context, Stack stack)
        {
            var value = stack.Pop();

            Apply(context, stack, value);
        }

        public void Close(Context context, Stack stack)
        {
            var value = stack.Pop();

            Close(context, stack, value);
        }

        public void Curry(Context context, Stack stack)
        {
            var quotation = stack.Pop();
            var value = stack.Pop();

            stack.Push(Closure.From(context, quotation, value, Symbol.ApplySym));
        }


        public void If(Context context, Stack stack)
        {
            var ifElse = stack.Pop();
            var ifTrue = stack.Pop();
            var condition = stack.Pop();

            Apply(context, stack, condition);
            var result = stack.Pop();

            if (result is bool boolean && boolean)
            {
                Apply(context, stack, ifTrue);
            }
            else
            {
                Apply(context, stack, ifElse);
            }
        }
    }
}
