using System;

namespace CoTy.Errors
{
    public class TypeMismatchException : ApplicationException
    {
        public TypeMismatchException(string message)
            : base(message)
        {
        }
    }
}
