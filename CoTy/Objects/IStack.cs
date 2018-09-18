namespace CoTy.Objects
{
    public interface IStack
    {
        int Count { get; }

        void Check(int expected);
        void Clear();
        void Drop();
        void Dump();
        void Dup();
        Sequence Get();
        void Over();
        object Pop();
        void Push(object value);
        void Swap();
    }
}