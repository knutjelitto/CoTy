namespace Pliant.Ebnf
{
    public class EbnfLexerRule : EbnfNode
    {
        private readonly int _hashCode;
        public EbnfQualifiedIdentifier QualifiedIdentifier { get; }
        public EbnfLexerRuleExpression Expression { get; }

        public EbnfLexerRule(EbnfQualifiedIdentifier qualifiedIdentifier, EbnfLexerRuleExpression expression)
        {
            QualifiedIdentifier = qualifiedIdentifier;
            Expression = expression;
            this._hashCode = (QualifiedIdentifier, Expression).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfLexerRule>(obj, out var other) &&
                   (QualifiedIdentifier, Expression).Equals((other.QualifiedIdentifier, other.Expression));
        }

        public override int GetHashCode() => this._hashCode;
    }
}