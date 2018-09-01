using System;

namespace CoTy.Errors
{
    public class ScopeException : ApplicationException
    {
        public ScopeException(string message)
            : base(message)
        {
        }
    }
}
