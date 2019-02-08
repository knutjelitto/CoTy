using System.Diagnostics;

namespace Pliant.Grammars
{
    public sealed class LexerRuleType
    {
        public string Id { get; }

        private readonly int _hashCode;

        public LexerRuleType(string id)
        {
            Id = id;
            _hashCode = id.GetHashCode();
        }

        private static int ComputeHashCode(string id)
        {
            return id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is LexerRuleType other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this._hashCode;
        }

        public static bool operator ==(LexerRuleType first, LexerRuleType second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(LexerRuleType first, LexerRuleType second)
        {
            return !first.Equals(second);
        }
    }
}