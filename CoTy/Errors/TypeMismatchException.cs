namespace CoTy.Errors
{
    public class TypeMismatchException : CotyException
    {
        public TypeMismatchException(string message)
            : base(message)
        {
        }
    }
}
