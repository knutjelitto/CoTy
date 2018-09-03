using System;

namespace CoTy.Errors
{
    public class BinderException : ApplicationException
    {
        public BinderException(string message)
            : base(message)
        {
        }
    }
}
