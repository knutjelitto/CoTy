using System.Diagnostics;

namespace Pliant.Tokens
{
    public sealed class TokenType
    {
        public string Id { get; }
        private readonly int _hashCode;

        public TokenType(string id)
        {
            Id = id;
            this._hashCode = Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is TokenType other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this._hashCode;
        }

        public override string ToString()
        {
            return Id;
        }

        public static bool operator ==(TokenType first, TokenType second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(TokenType first, TokenType second)
        {
            return !first.Equals(second);
        }
    }
}