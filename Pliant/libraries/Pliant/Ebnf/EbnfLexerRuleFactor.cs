namespace Pliant.Ebnf
{
    public abstract class EbnfLexerRuleFactor : EbnfNode
    {
    }

    public class EbnfLexerRuleFactorLiteral : EbnfLexerRuleFactor
    {
        private readonly int _hashCode;
        public string Value { get; }

        public EbnfLexerRuleFactorLiteral(string value)
        {
            Value = value;
            this._hashCode = Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfLexerRuleFactorLiteral>(obj, out var other) &&
                   Value.Equals(other.Value);
        }

        public override int GetHashCode() => this._hashCode;
    }

    public class EbnfLexerRuleFactorRegex : EbnfLexerRuleFactor
    {
        private readonly int _hashCode;
        public RegularExpressions.Regex Regex { get; }

        public EbnfLexerRuleFactorRegex(RegularExpressions.Regex regex)
        {
            Regex = regex;
            this._hashCode = Regex.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfLexerRuleFactorRegex>(obj, out var other) &&
                   Regex.Equals(other.Regex);
        }

        public override int GetHashCode() => this._hashCode;
    }
}