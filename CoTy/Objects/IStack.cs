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
        Cobject Pop();
        void Push(Cobject value);
        void Swap();
    }
}