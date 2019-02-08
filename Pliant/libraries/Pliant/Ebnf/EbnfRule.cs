namespace Pliant.Ebnf
{
    public class EbnfRule : EbnfNode
    {
        private readonly int _hashCode;
        public EbnfQualifiedIdentifier QualifiedIdentifier { get; }
        public EbnfExpression Expression { get; }

        public EbnfRule(EbnfQualifiedIdentifier qualifiedIdentifier, EbnfExpression expression)
        {
            QualifiedIdentifier = qualifiedIdentifier;
            Expression = expression;
            this._hashCode = (QualifiedIdentifier, Expression).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfRule>(obj, out var other) &&
                   (QualifiedIdentifier, Expression).Equals((other.QualifiedIdentifier, other.Expression));
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return $"{QualifiedIdentifier} = {Expression}";
        }
    }
}