namespace CoTy.Objects
{
    public partial class Character : Cobject<char, Character>, IOrdered<Character>
    {
        public Character(char value) : base(value)
        {
        }
    }
}
