namespace CoTy.Objects
{
    public interface IComparable<in T> where T : Cobject
    {
        // ReSharper disable once UnusedMember.Global
        Bool Equals(T other);
    }
}
