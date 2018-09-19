namespace CoTy.Errors
{
    public class StackException : CotyException
    {
        public StackException(int expected, int actual)
            : this(FmtMessage(expected, actual))
        {
        }

        private StackException(string message)
            : base(message)
        {
        }

        private static string FmtMessage(int expected, int actual)
        {
            if (expected > 1)
            {
                return $"stack underflow - expected {expected} arguments (got {actual})";
            }
            return $"stack underflow - expected {expected} argument (got {actual})";
        }
    }
}
