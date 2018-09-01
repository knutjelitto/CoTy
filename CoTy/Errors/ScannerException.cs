using System;

namespace CoTy.Errors
{
    public class ScannerException : ApplicationException
    {
        public ScannerException(string message)
            : base(message)
        {
        }
    }
}
