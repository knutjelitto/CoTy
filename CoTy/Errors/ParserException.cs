using System;

namespace CoTy.Errors
{
    public class ParserException : ApplicationException
    {
        public ParserException(string message)
            : base(message)
        {
        }
    }
}
