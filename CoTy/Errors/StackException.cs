namespace CoTy.Errors
{
    public class StackException : CotyException
    {
        public StackException(int expected, int actual)
            : this($"stack underflow - expected at least {expected} arguments (got {actual})")
        {
        }

        private StackException(string message)
            : base(message)
        {
        }
    }
}
