using System;

namespace CoTy.Errors
{
    public class CotyException : ApplicationException
    {
        public CotyException(string message)
            : base(message)
        {
        }
    }
}
