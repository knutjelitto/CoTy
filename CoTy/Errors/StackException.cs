using System;

namespace CoTy.Errors
{
    public class StackException : ApplicationException
    {
        public StackException(string message)
            : base(message)
        {
        }
    }
}
